using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class UsersRepository
    {
        DatabaseContext _db;

        public UsersRepository(DatabaseContext db)
        {
            _db = db;
        }

        public List<ApplicationUser> List()
        {
            var users = _db.Users.ToList();
            return users;
        }
    }
}