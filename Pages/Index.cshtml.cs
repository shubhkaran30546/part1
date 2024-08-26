using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;

namespace part1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public int VisitCount { get; private set; }
        public string? IpAddress { get; private set; }
        public string? TimeZone { get; private set; }

        public void OnGet()
        {
            // Get or create the persistent cookie
            VisitCount = 0;
            if (Request.Cookies.ContainsKey("VisitCount"))
            {
                bool parseSuccess = int.TryParse(Request.Cookies["VisitCount"], out int count);
                if (parseSuccess)
                {
                    VisitCount = count;
                }
                VisitCount++;
            }
            else
            {
                VisitCount = 1;
            }

            // Set the cookie with an expiration date
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true
            };
            Response.Cookies.Append("VisitCount", VisitCount.ToString(), cookieOptions);

            // Get the client IP address
            IpAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault() ?? Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            // Get the client's time zone
            TimeZone = TimeZoneInfo.Local.StandardName;
        }
    }
}
