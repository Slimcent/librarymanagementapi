using Library.Entities.Entities;
using Library.LoggerService;
using Library.Repository.Interfaces;
using Library.Services.Interfaces;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IRepository<User> _userRepo;
        private readonly ILoggerManager _logger;

        private string DeletedMessage { get; set; }

        public UserService(IUnitofWork unitOfWork, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<User>();
            _logger = logger;
        }

        public async Task Add(User user)
        {
            await _userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User> GetUserByID(object Id)
        {
            var user = await _userRepo.GetAsync(Id);
            if (user.IsDeleted)
            {
                _logger.LogWarn("No user was Found");
                return null;
            }
            return user;
        }

        public string DeleteUser(string Id)
        {
            User user = (User)_userRepo.Find(a => a.Id == Id);
            int deletedItems = 0;
            try
            {
                if (user != null)
                {
                    _userRepo.Delete(user);
                    deletedItems = _unitOfWork.SaveChanges();
                    if (deletedItems > 0)
                    {
                        DeletedMessage = $"User  was Deleted Successfully";
                    }
                    else
                    {
                        DeletedMessage = $"Deleting User failed";
                    }
                }
            }
            catch (DbException)
            {
                throw;
            }
            return DeletedMessage;
        }

        public string DeleteUser(User user)
        {
            return DeleteUser(user.Id);
        }

        public async Task<User> UpdateUser(User user)
        {
            _userRepo.Update(user);
            var changes = await _unitOfWork.SaveChangesAsync();
            if (changes > 0)
                return user;
            else
                return null;
        }

        public async Task DeleteOrRetrieveUser(User user)
        {
            if (user.IsDeleted) 
                user.IsDeleted = false;
            else
                user.IsDeleted = true;

            await _unitOfWork.SaveChangesAsync();
        }
               
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.GetAll().AsEnumerable();
        }

        public async Task Save() => await _userRepo.RepoSaveAsync();
    }
}
