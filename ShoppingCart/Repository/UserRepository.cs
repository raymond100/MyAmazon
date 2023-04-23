using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(DataContext context, UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public Status ApproveUser(string UserId)
        {
            AppUser user = _context.Users.Single(u => u.Id.Equals(UserId));
            user.IsAproved = true;
            _context.SaveChanges();
            Status status = new Status();
            status.StatusCode = 1;
            status.Message = "User Approved";
            return status;
        }

       public async Task<List<AppUser>> GetAllNonApprovedUsers()
        {
            var users = await _context.Users
                .Where(u => !u.IsAproved)
                .ToListAsync();

            foreach (var user in users)
            {
                if(user != null){
                    var roles = await _userManager.GetRolesAsync(user);

                    user.Roles = new List<IdentityUserRole<string>>();
                    foreach (var role in roles)
                    {
                        user.Roles.Add(new IdentityUserRole<string> { RoleId = role });
                    }
                }
            }

            return users;
        }



    }
}
