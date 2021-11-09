using System;
using Microsoft.AspNetCore.Identity;

namespace RestBackend.Core.Models.Auth
{
    public class Role : IdentityRole<Guid> { }

    public class RoleClaim : IdentityRoleClaim<Guid> { }

}