using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Repository
{
    public interface IUserRepository
    {
        public List<AppUser> GetAllNonApprovedUsers();
        public Status ApproveUser(string UserId);
        public Status SaveUserAccount (UserAccount account);
    }
}
