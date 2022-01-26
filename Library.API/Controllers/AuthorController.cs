using AutoMapper;
using Library.Entities.Entities;
using Library.Entities.Entities.DTO;
using Library.LoggerService;
using Library.Repository.Interfaces;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ILoggerManager _logger;
        private readonly IUnitofWork _unitOfWork;

        public AuthorController(IBookService bookService, IAuthorService authorService,
            IUnitofWork unitOfWork, IMapper mapper, ILoggerManager logger)
        {
            _bookService = bookService;
            _mapper = mapper;
            _authorService = authorService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        //[Authorize(Policy = "AdminRolePolicy")]
        public IActionResult GetAllBooks()
        {
            var allBooks = _bookService.GetAllBooks();
            var booksDto = _mapper.Map<IEnumerable<BookGetDto>> (allBooks);
            return Ok(booksDto);
        }

        [HttpGet]
        public IActionResult GetAuthurBook(Guid id)
        {
            var allBooks = _bookService.Get(id);
            var authorBooks = _authorService.GetAuthorByIdAsync(allBooks.AuthorId);
            return Ok(authorBooks);
        }

        [HttpGet("{id}", Name = "BookId")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookById = await _bookService.GetBookByIDAsync(id);
            if (bookById == null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in our database.");
                return NotFound();
            }
            else
            {
                return Ok(bookById);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookDto book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Models");
                return UnprocessableEntity(ModelState);
            }
            var authorId = await _authorService.GetAuthorByIdAsync(book.AuthorId);
            if (authorId == null)
            {
                _logger.LogWarn("Author does not exist");
                return BadRequest("Author is null");
            }
            var bookEntity = _mapper.Map<Book>(book);
            await _bookService.AddBookAsync(bookEntity);

            var bookToReturn = _mapper.Map<DisplayBookDto>(bookEntity);
            return CreatedAtRoute("BookId", new { id = bookToReturn.Id },
            bookToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var bookToDelete = await _bookService.GetBookByIDAsync(id);
            if (bookToDelete == null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _bookService.DeleteBook(bookToDelete);
            
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookUpdateDto book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Models");
                return UnprocessableEntity(ModelState);
            }
            if (id != book.Id)
            {
                return BadRequest();
            }
            var bookToUpdate = await _bookService.GetBookByIDAsync(id);
            if (bookToUpdate == null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            
            _mapper.Map(book, bookToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();

            throw new Exception();
        }
    }
}
