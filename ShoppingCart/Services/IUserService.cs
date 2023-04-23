using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Services
{
    public interface IUserService
    {
        //Task<List<AppUser>> GetNonApprovedUsers();
        public Status ApproveUser(string UserId);
        Task<List<AppUser>> GetAllNonApprovedUsers();
    }
}
