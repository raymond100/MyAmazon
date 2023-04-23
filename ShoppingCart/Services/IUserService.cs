using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Services
{
    public interface IUserService
    {
        public List<AppUser> GetAllNonApprovedUsers();
        public Status ApproveUser(string UserId);
        public Status SaveUserAccount(UserAccount Account);
    }
}
