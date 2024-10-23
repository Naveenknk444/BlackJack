using BlackJackGameLogic.Interfaces;
using BlackJackGameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUser(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public void RegisterUser(User user)
        {
            _userRepository.Add(user);
        }
    }

}
