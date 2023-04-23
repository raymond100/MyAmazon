using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) {
            _context = context;
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

        public  List<AppUser> GetAllNonApprovedUsers()
        {
           List<AppUser> list =  _context.Users.Where(u=>u.IsAproved == false).ToList();
            return list;
        }

        public Status SaveUserAccount(UserAccount account)
        {
            Status status = new Status();
            _context.UsersAccounts.Add(account);
            _context.SaveChanges();
            status.Message = "Account Saved";
            status.StatusCode=1;
            return status;
        }
    }
}
