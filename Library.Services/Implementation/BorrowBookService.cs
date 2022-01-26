using Library.Entities.Entities;
using Library.Repository.Interfaces;
using Library.Services.Implementation;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public class BorrowBookService : IBorrowBookService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IRepository<BorrowBook> _bookRepo;

        public BorrowBookService(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bookRepo = unitOfWork.GetRepository<BorrowBook>();
        }
        public async Task AddBookAsync(BorrowBook book)
        {
            await _bookRepo.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
