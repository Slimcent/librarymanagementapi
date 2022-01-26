using Library.Entities.Entities;
using Library.Repository.Interfaces;
using System.Threading.Tasks;

namespace Library.Services.Implementation
{
    public interface IBorrowBookService
    {
        Task AddBookAsync(BorrowBook book);
    }
}
