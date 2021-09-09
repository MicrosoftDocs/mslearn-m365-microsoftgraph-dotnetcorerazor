using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreRazor_MSGraph.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace DotNetCoreRazor_MSGraph.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class CalendarModel : PageModel
    {
        private readonly ILogger<CalendarModel> _logger;
        private readonly GraphCalendarClient _graphCalendarClient;
        private readonly GraphProfileClient _graphProfileClient;
        private MailboxSettings MailboxSettings { get; set; }

        public IEnumerable<Event> Events  { get; private set; }

        public CalendarModel(ILogger<CalendarModel> logger, GraphCalendarClient graphCalendarClient, GraphProfileClient graphProfileClient)
        {
            _logger = logger;
            _graphCalendarClient = graphCalendarClient;
            _graphProfileClient = graphProfileClient;
        }

        public async Task OnGetAsync()
        {
            // Remove this code
           await Task.CompletedTask;
        }

        public string FormatDateTimeTimeZone(DateTimeTimeZone value)
        {
            // Parse the date/time string from Graph into a DateTime
            var graphDatetime = value.DateTime;
            if (DateTime.TryParse(graphDatetime, out DateTime dateTime)) 
            {
                var dateTimeFormat = $"{MailboxSettings.DateFormat} {MailboxSettings.TimeFormat}".Trim();
                if (!String.IsNullOrEmpty(dateTimeFormat)) {
                    return dateTime.ToString(dateTimeFormat);
                }
                else 
                {
                    return $"{dateTime.ToShortDateString()} {dateTime.ToShortTimeString()}";
                }
            }
            else
            {
                return graphDatetime;
            }
        }
    }
}
