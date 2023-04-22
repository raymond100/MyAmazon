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
        public List<AppUser> GetAllNonApprovedUsers()
        {
            return _userRepository.GetAllNonApprovedUsers();
        }
        public Status ApproveUser(string UserId) { 
        
         return _userRepository.ApproveUser(UserId);    
            
        }
    }
}
