using RestBackend.Core.Models.Auth;
using RestBackend.Core.Resources;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestBackend.Core.Services.Infrastructure
{
    public interface IJWTService
    {
        TokenResource GenerateJwt(User user, IList<string> roles);

        TokenResource GenerateJwt(User user, IList<string> roles, IList<Claim> permissions);
    }
}
