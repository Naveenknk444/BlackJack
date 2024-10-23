using BlackJackGameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Interfaces
{
    public interface IUserService
    {
        User GetUser(int userId);
        void RegisterUser(User user);
    }

}
