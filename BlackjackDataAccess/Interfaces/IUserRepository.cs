using BlackJackGameLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackjackDataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
