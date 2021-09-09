
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Linq;

namespace DotNetCoreRazor_MSGraph.Graph
{
    public class GraphFilesClient
    {
        private readonly ILogger<GraphFilesClient> _logger = null;
        private readonly GraphServiceClient _graphServiceClient = null;

        public GraphFilesClient()
        {
            // Remove this code
            _ = _logger;
            _ = _graphServiceClient;
        }

        public async Task<IDriveItemChildrenCollectionPage> GetFiles()
        {
            try
            {

                // Remove this code
                return await Task.FromResult<IDriveItemChildrenCollectionPage>(null);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Graph /me/drive/root/children: {ex.Message}");
                throw;
            }
        }

        public async Task<Stream> DownloadFile(string fileId)
        {
            try
            {

                // Remove this code
                return await Task.FromResult<Stream>(null);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading file: {ex.Message}");
                throw;
            }
        }

        public async Task UploadFile(string fileName, Stream stream)
        {
            var itemPath = Uri.EscapeUriString(fileName);
            var size = stream.Length / 1000;
            _logger.LogInformation($"Stream size: {size} KB");
            if (size/1000 > 4)
            {
                // Allows slices of a large file to be uploaded 
                // Optional but supports progress and resume capabilities if needed
                await UploadLargeFile(itemPath, stream);
            }
            else
            {
                try
                {
                    // Uploads entire file all at once. No support for reporting progress.
                    var driveItem = await _graphServiceClient.Me.Drive.Root.ItemWithPath(itemPath)
                        .Content
                        .Request()
                        .PutAsync<DriveItem>(stream);
                    _logger.LogInformation($"Upload complete: {driveItem.Name}");
                }
                catch (ServiceException ex)
                {
                    _logger.LogError($"Error uploading: {ex.ToString()}");
                    throw;
                }
            }
        }

        private async Task UploadLargeFile(string itemPath, Stream stream)
        {
            // Allows "slices" of a file to be uploaded.
            // This technique provides a way to capture the progress of the upload
            // and makes it possible to resume an upload using fileUploadTask.ResumeAsync(progress);
            // Based on https://docs.microsoft.com/en-us/graph/sdks/large-file-upload

            // Use uploadable properties to specify the conflict behavior (replace in this case).
            var uploadProps = new DriveItemUploadableProperties
            {
                ODataType = null,
                AdditionalData = new Dictionary<string, object>
                {
                    { "@microsoft.graph.conflictBehavior", "replace" }
                }
            };

            // Create the upload session


            try
            {
                // Remove this code
                await Task.CompletedTask;

            }
            catch (ServiceException ex)
            {
                _logger.LogError($"Error uploading: {ex.ToString()}");
                throw;
            }
        }
    }
}