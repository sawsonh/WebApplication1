using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Demo.Core.Services
{
    public interface IUserService
    {
        IEnumerable<Claim> GetClaims(ClaimsIdentity identity);
    }
}
