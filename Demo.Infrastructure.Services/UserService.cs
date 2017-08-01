using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Demo.Core.Constants;
using Demo.Core.Repositories;
using Demo.Core.Services;

namespace Demo.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IConfigService _cfgSvc;
        private readonly IUserRepository _userRepo;

        public UserService(IConfigService cfgSvc, IUserRepository userRepo)
        {
            _cfgSvc = cfgSvc;
            _userRepo = userRepo;
        }

        public IEnumerable<Claim> GetClaims(ClaimsIdentity identity)
        {
            var usernameClaim =
                identity.Claims.FirstOrDefault(
                    c => c.Issuer.Equals(_cfgSvc.GetValue<string>(ConfigurationKeys.Authority)) &&
                         c.Type.Equals("preferred_username"))?.Value;
            if (string.IsNullOrEmpty(usernameClaim))
                return Enumerable.Empty<Claim>();
            var user = _userRepo.FirstOrDefault(
                usr => usr.Username.Equals(usernameClaim, StringComparison.OrdinalIgnoreCase));
            if (user == null || user.Roles == null)
                return Enumerable.Empty<Claim>();
            return user.Roles.Select(kvp => new Claim(kvp.Key, kvp.Value));
        }
    }
}
