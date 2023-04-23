using ShoppingCart.Models;
using ShoppingCart.Repository;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository; 
        }
        public async Task<List<AppUser>> GetAllNonApprovedUsers()
        {
            List<AppUser> users = await _userRepository.GetAllNonApprovedUsers();
            return users;
        }

        public Status ApproveUser(string UserId) { 
        
         return _userRepository.ApproveUser(UserId);    
            
        }


        public Status SaveUserAccount(UserAccount Account)
        {
            return _userRepository.SaveUserAccount(Account);
        }
    }
}
