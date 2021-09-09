using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreRazor_MSGraph.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace DotNetCoreRazor_MSGraph.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    // Max supported upload size is 100MB
    [RequestFormLimits(MultipartBodyLengthLimit = 100000000)]
    [RequestSizeLimit(100000000)]
    public class FilesModel : PageModel
    {
        private readonly ILogger<FilesModel> _logger;
        private readonly GraphFilesClient _graphFilesClient;

        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        public IDriveItemChildrenCollectionPage Files { get; private set; }

        public FilesModel(ILogger<FilesModel> logger, GraphFilesClient graphFilesClient)
        {
            _graphFilesClient = graphFilesClient;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Files = await _graphFilesClient.GetFiles(); 
        }

        public async Task OnPostAsync()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                return;
            }
 
            _logger.LogInformation($"Uploading {UploadedFile.FileName}.");

            using (var stream = new MemoryStream()) {
                await UploadedFile.CopyToAsync(stream);
                await _graphFilesClient.UploadFile(UploadedFile.FileName, stream);
            }
            await OnGetAsync();
        }

        public async Task<FileStreamResult> OnGetDownloadFile(string id, string name) {
            var stream = await _graphFilesClient.DownloadFile(id);
            return File(stream, MediaTypeNames.Application.Octet, name);
        }
    }
}
