using Library.Entities.Entities;
using Library.Repository.Interfaces;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IRepository<Book> _bookRepo;

        private string DeletedMessage { get; set; }

        public BookService(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bookRepo = unitOfWork.GetRepository<Book>();
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepo.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Book> GetBookByIDAsync(Guid Id)
        {
            return await _bookRepo.GetAsync(Id);
        }

        public Book GetBookByTitle(string Title)
        {
           
            return _bookRepo.Find(a => a.Title == Title).FirstOrDefault();
        }
        
        public Book GetBookByAuthorName(string authorName)
        {
            return _bookRepo.Find(a => a.Author.User.UserName == authorName).FirstOrDefault();
        }
        
        public IEnumerable<Book> GetBooksByCategory(string category)
        {
            return _bookRepo.Find(a => a.Category == category).ToList();
        }
        public string DeleteBook(Guid Id)
        {
            Book toDelete = (Book)_bookRepo.Find(a => a.Id == Id);
            int deletedItems = 0;
            try
            {
                if (toDelete != null)
                {
                    _bookRepo.Delete(toDelete);
                    deletedItems = _unitOfWork.SaveChanges();
                    if (deletedItems > 0)
                    {
                        DeletedMessage = $"User  was Deleted Successfully";
                    }
                    else
                    {
                        DeletedMessage = $"Deleting User  failed";
                    }
                }
            }
            catch (DbException)
            {
                throw;
            }
            return DeletedMessage;
        }

        public string DeleteBook(Book book)
        {
            return DeleteBook(book);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _bookRepo.Update(book);
            var changes = await _unitOfWork.SaveChangesAsync();
            if (changes > 0)
                return book;
            else
                return null;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepo.GetAll().AsEnumerable();
        }

        public Book Get(Guid Id)
        {
            return _bookRepo.Find(a => a.Id == Id).FirstOrDefault();
        }

        
    }
}
