using Library.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IBookService
    {
        Task AddBookAsync(Book book);
        Task<Book> GetBookByIDAsync(Guid Id);
        Book GetBookByTitle(string Title);

        Book GetBookByAuthorName(string authorName);
        IEnumerable<Book> GetAllBooks();
        Task<Book> UpdateBook(Book user);
        IEnumerable<Book> GetBooksByCategory(string Category);
        string DeleteBook(Guid Id);
        string DeleteBook(Book user);
        public Book Get(Guid Id);
    }
}
