using BlackJackGameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int userId);
        void Add(User user);
    }

}
