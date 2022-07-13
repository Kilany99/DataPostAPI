using DataPostAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DataPostAPI.Handlers
{
    public class _AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ClientContext _context;
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authoraization"))
                return AuthenticateResult.Fail("Authoraization was not found !");
            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authoraization"]);
            var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
            string [] credentials = Encoding.UTF8.GetString(bytes).Split(":");
            string adminName = credentials[0];
            string adminPass = credentials[1];
            AdminModel admin = _context.Admins.Where(admin => admin.Admin_id == adminName && admin.Ad_Password == adminPass).FirstOrDefault();
            return AuthenticateResult.Fail("Need to implement !");

        }
        public _AuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ClientContext context) : base (options,logger,encoder,clock)
        {

            _context = context;
        }
    }
}
