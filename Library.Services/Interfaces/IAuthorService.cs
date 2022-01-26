using Library.Entities.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IAuthorService
    {
        Task SoftDeleteOrRetrieveAuthor(Author author);
        Task AddAuthor(User user);
        Task<Author> GetAuthorByIdAsync(object Id);
        Author GetAuthorByName(string username);
        IEnumerable<string> GetAllAuthors();        
        Task Save();
    }
}
