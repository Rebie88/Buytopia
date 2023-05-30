using Buytopia.Models;
using Buytopia.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Buytopia.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //constuctor 
        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        // if any pending migrations then add migration
        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            //if role exist do nothing 
            if (_db.Roles.Any(r => r.Name == SF.ManagerUser)) return;
            //if role doesn't exist create all the roles 
            _roleManager.CreateAsync(new IdentityRole(SF.ManagerUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SF.EmployeeUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SF.CustomerEndUser)).GetAwaiter().GetResult();

            //create default manager user 
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "buytopia23@gmail.com",
                Email = "buytopia23@gmail.com",
                Name = "Rebie Seid",
                EmailConfirmed = true,
                PhoneNumber = "2062223333"
            }, "Admin123*").GetAwaiter().GetResult();


            //retrive the user from seeded database
            IdentityUser user = await _db.Users.FirstOrDefaultAsync(u => u.Email == "buytopia23@gmail.com");

            //assign this user the role of manager user.
            await _userManager.AddToRoleAsync(user, SF.ManagerUser);

        }
    }
}
