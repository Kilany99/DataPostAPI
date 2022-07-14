using DataPostAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
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

            try
            {

                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authoraization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string adminName = credentials[0];
                string adminPass = credentials[1];
                AdminModel admin = _context.Admins.Where(admin => admin.Admin_id == adminName && admin.Ad_Password == adminPass).FirstOrDefault();
                
                if(admin == null)
                    return AuthenticateResult.Fail("Invalid username or password !");
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, adminName) };
                    var identity = new[] { new ClaimsIdentity(claims, Scheme.Name) };
                    var principal =  new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket (principal,Scheme.Name);
                    return AuthenticateResult.Success(ticket);

                }

            }
            catch (Exception)
            {

                return AuthenticateResult.Fail("Error has occured !");
            }



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
