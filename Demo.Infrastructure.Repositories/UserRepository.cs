using System;
using System.Collections.Generic;
using System.Text;
using Demo.Core.Entities;
using Demo.Core.Repositories;

namespace Demo.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository()
        {
            base.AddAll(new List<User>
            {
                new User
                {
                    Username = "pbansal@flexerasoftware.com",
                    Roles = new Dictionary<string, string> {{"UserGroup", "Admin"}}
                }
            });
        }
    }
}
