
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Linq;
using System.Net;

namespace DotNetCoreRazor_MSGraph.Graph
{
    public class GraphProfileClient
    {
        private readonly ILogger<GraphProfileClient> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public GraphProfileClient(ILogger<GraphProfileClient> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }
        public async Task<User> GetUserProfile()
        {
            User currentUser = null;

            try
            {
                currentUser = await _graphServiceClient
                    .Me
                    .Request()
                    .Select(u => new {
                        u.DisplayName
                    })
                    .GetAsync();
                    
                return currentUser;
            }
            // Catch CAE exception from Graph SDK
            catch (ServiceException ex) when (ex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
            {
                _logger.LogError($"/me Continuous access evaluation resulted in claims challenge: {ex.Message}");
                throw;
            }
            catch (Exception ex) {
                _logger.LogError($"/me Error: {ex.Message}");
                throw;
            }
        }
        public async Task<string> GetUserProfileImage()
        {
            try
            {
                // Get user photo
                using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photo = ((MemoryStream)photoStream).ToArray();
                    return Convert.ToBase64String(photo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Graph /me/photo: {ex.Message}");
                return null;
            }
        }

        public async Task<Presence> GetUserPresence() {
            try
            {
                var presence = await _graphServiceClient.Me.Presence.Request().GetAsync();
                return presence;
            }
            // Catch CAE exception from Graph SDK
            catch (ServiceException ex) when (ex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
            {
                _logger.LogError($"/me/presence Continuous access evaluation resulted in claims challenge: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Graph /me/presence: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetPresenceCssClass()
        {
            var presence = await GetUserPresence();
            if (presence == null)
            {
                return "label-default";
            }

            UserPresence userPresence = Enum.Parse<UserPresence>(presence.Availability);

            switch (userPresence)
            {
                case UserPresence.Available:
                    return "border-success";
                case UserPresence.Away:
                case UserPresence.Busy:
                case UserPresence.Offline:
                case UserPresence.Unknown:
                case UserPresence.Invisible:
                case UserPresence.DoNotDisturb:
                case UserPresence.Meeting:
                    return "border-danger";
                default:
                    return "border-secondary";
            }
        }

    }

    public enum UserPresence {
        Available,
        Away,
        Busy,
        Offline,
        Unknown,
        Invisible,
        DoNotDisturb,
        Meeting
    }
}