
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Linq;
using System.Net;
using TimeZoneConverter;

namespace DotNetCoreRazor_MSGraph.Graph
{
    public class GraphCalendarClient
    {
        private readonly ILogger<GraphCalendarClient> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public GraphCalendarClient(ILogger<GraphCalendarClient> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        public async Task<IEnumerable<Event>> GetEvents(string userTimeZone)
        {
            _logger.LogInformation($"User timezone: {userTimeZone}");
            // Configure a calendar view for the current week
            var startOfWeek = DateTime.Now;
            var endOfWeek = startOfWeek.AddDays(7);

            var viewOptions = new List<QueryOption>
            {
                new QueryOption("startDateTime", startOfWeek.ToString("o")),
                new QueryOption("endDateTime", endOfWeek.ToString("o"))
            };

            try
            {
                // Use the injected GraphServiceClient object to call Me.CalendarView
                var calendarEvents = await _graphServiceClient
                    .Me
                    .CalendarView
                    .Request(viewOptions)
                    .Header("Prefer", $"outlook.timezone=\"{userTimeZone}\"")
                    .Select(evt => new
                    {
                        evt.Subject,
                        evt.Organizer,
                        evt.Start,
                        evt.End
                    })
                    .OrderBy("start/DateTime")
                    .GetAsync();

                return calendarEvents;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Graph /me/calendaview: {ex.Message}");
                throw;
            }
        }

        // Used for timezone settings related to calendar
        public async Task<MailboxSettings> GetUserMailboxSettings()
        {
            try
            {
                var currentUser = await _graphServiceClient
                    .Me
                    .Request()
                    .Select(u => new
                    {
                        u.MailboxSettings
                    })
                    .GetAsync();

                return currentUser.MailboxSettings;
            }
            catch (Exception ex)
            {
                _logger.LogError($"/me Error: {ex.Message}");
                throw;
            }
        }

        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today, string timeZoneId)
        {
            // Time zone returned by Graph could be Windows or IANA style
            // .NET Core's FindSystemTimeZoneById needs IANA on Linux/MacOS,
            // and needs Windows style on Windows.
            // TimeZoneConverter can handle this for us
            TimeZoneInfo userTimeZone = TZConvert.GetTimeZoneInfo(timeZoneId);

            // Assumes Sunday as first day of week
            int diff = System.DayOfWeek.Sunday - today.DayOfWeek;

            // create date as unspecified kind
            var unspecifiedStart = DateTime.SpecifyKind(today.AddDays(diff), DateTimeKind.Unspecified);

            // convert to UTC
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, userTimeZone);
        }

    }
}