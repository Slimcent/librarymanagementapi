using Library.Entities.Entities;
using Library.Repository.Interfaces;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IRepository<Author> _authorRepo;
        private readonly UserManager<User> _userManager;

        private string DeletedMessage { get; set; }

        public AuthorService(IUnitofWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _authorRepo = unitOfWork.GetRepository<Author>();
            _userManager = userManager;
        }

        public async Task SoftDeleteOrRetrieveAuthor(Author author)
        {
            if (author.IsDeleted)
                author.IsDeleted = false;
            else
                author.IsDeleted = true;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddAuthor(User user)
        {
            Author author = new Author
            {
                CreatedAt = DateTime.Now,
                UserId = user.Id
            };
                        
            await _authorRepo.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();
        }
                
        public IEnumerable<string> GetAllAuthors()
        {
            var authors = _authorRepo.GetAll().Include(b => b.User).AsEnumerable();
            var Usernames = new List<string>();

            if (authors.Any())
            {
                foreach (var a in authors)
                {
                    if (a.UserId != null)
                    {
                        var name = a.User.UserName;
                        Usernames.Add(name);
                    }
                }
                if (!Usernames.Any())
                    return null;
            }
            return Usernames;
        }

        public async Task<Author> GetAuthorByIdAsync(object Id)
        {
            var author = await _authorRepo.GetAsync(Id);
            var user = await _userManager.FindByIdAsync(author.UserId);
            if (author.IsDeleted || user.IsDeleted)
                return null;
            return author;

        }

        public Author GetAuthorByName(string AuthorName)
        {
            return _authorRepo.Find(a => a.User.UserName == AuthorName).FirstOrDefault();
        }
                
        public async Task Save() => await _authorRepo.RepoSaveAsync();

    }
}
