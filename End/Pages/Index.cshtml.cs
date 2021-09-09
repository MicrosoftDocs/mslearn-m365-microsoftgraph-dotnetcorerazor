using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DotNetCoreRazor_MSGraph.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace DotNetCoreRazor_MSGraph.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class IndexModel : PageModel
    {
        private readonly GraphProfileClient _graphProfileClient;
        public string UserDisplayName { get; private set; } = "";
        public string UserPhoto { get; private set; }
        readonly ITokenAcquisition _tokenAcquisition;
        readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, GraphProfileClient graphProfileClient, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _graphProfileClient = graphProfileClient;
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task OnGetAsync()
        {
            var user = await _graphProfileClient.GetUserProfile(); 
            UserDisplayName = user.DisplayName.Split(' ')[0];
            UserPhoto = await _graphProfileClient.GetUserProfileImage();
        }

        public async Task OnGetAccessTokenAsync() {
            // A simple example of getting an access token and making a call to Graph to retrieve the 
            // user's display name. You can view the token in the console (after running dotnet run).
            // Visit https://jwt.ms, copy the token into the textbox, and you can see the scopes available to the 
            // token in addition to other information.
            // https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-web-app-call-api-acquire-token?tabs=aspnetcore

            // Acquire the access token.
            string[] scopes = new string[]{"user.read"};
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _logger.LogInformation($"Token: {accessToken}");

            // Use the access token to call a protected web API.
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string json = await client.GetStringAsync("https://graph.microsoft.com/v1.0/me?$select=displayName");
            _logger.LogInformation(json);
        }
    }
}
