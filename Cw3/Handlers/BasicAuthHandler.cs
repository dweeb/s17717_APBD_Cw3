using Cw3.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Cw3.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IDbAuthService _dbservice;
        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IDbAuthService service) : base(options, logger, encoder, clock)
        {
            _dbservice = service;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing auth header");
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credBytes).Split(":");
            if (credentials.Length != 2)
                return AuthenticateResult.Fail("Incorrect auth header value");
            string role = _dbservice.AuthenticateAndGetRole(credentials[0], credentials[1]);
            if (role == null)   // student needs a role and a password to authenticate
                return AuthenticateResult.Fail("Incorrect username or password"); 
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, credentials[0]),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
