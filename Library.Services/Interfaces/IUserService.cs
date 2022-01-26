using Library.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IUserService
    {

        Task DeleteOrRetrieveUser(User user);
        Task Add(User user);
        Task<User> GetUserByID(object Id);
        IEnumerable<User> GetAllUsers();
        Task<User> UpdateUser(User user);
        string DeleteUser(string Id);
        string DeleteUser(User user);
        Task Save();
    }
}
