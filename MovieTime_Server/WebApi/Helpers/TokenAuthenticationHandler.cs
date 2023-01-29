using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public class TokenAuthenticationHandler 
    {
        private readonly UserManager<IdentityUser> _userManager;
        public TokenAuthenticationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityUser> GetCurrentUserAsync(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity == null) return await Task.FromResult<IdentityUser>(null);

            var userName = identity.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
            return await _userManager.FindByNameAsync(userName);
        }
    
    }
}
