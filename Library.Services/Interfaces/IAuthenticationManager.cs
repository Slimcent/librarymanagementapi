using Library.Entities.Entities.DTO;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(LoginModel loginModel);
        Task<string> CreateToken();
    }
}
