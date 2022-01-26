using AutoMapper;
using Library.Entities.Entities;
using Library.Entities.Entities.DTO;
using Library.LoggerService;
using Library.Services.Implementation;
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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        private readonly IBorrowBookService _borrowBookService;
        private readonly IAuthorService _authorService;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public UserController(IBookService bookService, IAuthorService authorService, IUserService userService, 
            IBorrowBookService borrowBookService, IMapper mapper, ILoggerManager logger)
        {
            _bookService = bookService;
            _mapper = mapper;
            _authorService = authorService;
            _borrowBookService = borrowBookService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var allBooks = _bookService.GetAllBooks();
            var booksDto = _mapper.Map<IEnumerable<BookGetDto>>(allBooks);
           
            return Ok(booksDto);
        }


        [HttpGet("{title}")]
        public IActionResult GetBookByTitle(string title)
        {
            
            var booksTitle = _bookService.GetBookByTitle(title);
            if (booksTitle == null)
            {
                _logger.LogInfo($"Book with title: {title} doesn't exist in our database.");
                return NotFound();
            }
            else
            {
                return Ok(booksTitle);
            }
        }

        [HttpGet("{category}")]
        public IActionResult GetBooksByCategory(string category)
        {
            var booksCategory = _bookService.GetBooksByCategory(category);
            if (booksCategory == null)
            {
                _logger.LogInfo($"{category} category doesn't exist in our database.");
                return NotFound();
            }
            else
            {
                return Ok(booksCategory);
            }

        }

        [HttpPost]
        public async Task<IActionResult> BorrowBook([FromBody] BookBorrowPostDto book)
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
            var userId = await _userService.GetUserByID(book.UserId);
            if (userId == null)
            {
                _logger.LogWarn("User does not exist");
                return BadRequest("User is null");
            }

            var bookEntity = _mapper.Map<BorrowBook>(book);
            await _borrowBookService.AddBookAsync(bookEntity);

            var bookToReturn = _mapper.Map<BookBorrowGetDto>(bookEntity);
            return CreatedAtRoute("BookId", new { id = bookToReturn.Id },
            bookToReturn);
        }

        [HttpGet]
        public IActionResult GetAuthurBook(Guid id)
        {
            var allBooks = _bookService.Get(id);
            var authorBooks = _authorService.GetAuthorByIdAsync(allBooks.AuthorId);
            return Ok(authorBooks);
        }

        
        [HttpGet]
        public IActionResult GetBookByAuthorName (string authorName)
        {
            //var NameOfAuthor = _authorService.GetAuthorByName(authorName);
            var authorname = _bookService.GetBookByAuthorName(authorName);

            if (authorname == null)
            {
                _logger.LogInfo($"Book with title: {authorName} doesn't exist in our database.");
                return NotFound();
            }
            else
            {
                return Ok(authorname);
            }
        }
        
        
        


    }
}
