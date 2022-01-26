using AutoMapper;
using Library.API.Action_Filters;
using Library.Entities.Entities;
using Library.Entities.Entities.DTO;
using Library.Entities.Helpers;
using Library.LoggerService;
using Library.Repository.Interfaces;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    //[Authorize(Policy = "AdminRolePolicy")]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(AdminControllerValidation))]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly ILoggerManager _logger;
        private readonly UserManager<User> _userManager;

        public AdminController(IUserService userService, IAuthorService authorService, IBookService bookService,
            ILoggerManager logger, IMapper mapper, UserManager<User> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _authorService = authorService;
            _bookService = bookService;
            _logger = logger;
            _userManager = userManager;
                        
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers().ToList();
            var usersDto = _mapper.Map<IEnumerable<DisplayUserDataDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            User user;
            try
            {
                user = await _userService.GetUserByID(id);
            }
            catch (NullReferenceException e)
            {
                _logger.LogWarn($"User does not exist {e.Message}");
                return NoContent();
            }

            var userDto = _mapper.Map<DisplayUserDataDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.UserName);

            if (existingUser != null)
                return BadRequest($"User exists.");

            var newUser = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return BadRequest($"User exists.");
        
            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            User user = null;
            try
            {
                user = await _userService.GetUserByID(id);
            }
            catch(NullReferenceException e)
            {
                _logger.LogWarn($"User does not exist {e.Message}");
                return NoContent();
            }            

            await _userService.DeleteOrRetrieveUser(user);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UndeleteUserById(string id)
        {
            var user = await _userService.GetUserByID(id);
            if (user == null)
            {
                _logger.LogWarn("User does not exist");
                return NotFound();
            }
               
            await _userService.DeleteOrRetrieveUser(user);

            var userDto = _mapper.Map<DisplayUserDataDto>(user);
            return Ok(userDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] JsonPatchDocument<UserUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogWarn("patchDoc object sent from client is null.");
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarn("User does not exist");
                return NotFound();
            }
            var userPatch = _mapper.Map<UserUpdateDto>(user);
            patchDoc.ApplyTo(userPatch, ModelState);
            TryValidateModel(userPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(userPatch, user);
            await _userService.Save();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAuthor([FromBody] RegisterAuthorDto authorDto)
        {
            if (authorDto == null)
            {
                _logger.LogError("You are not allowed to create a null entity");
                return NoContent();
            }

            var user = await _userManager.FindByNameAsync(authorDto.Username);
            if (user == null || user.IsDeleted)
            {
                _logger.LogWarn("Author does not exist as a user");
                return NoContent();
            }

            if (await _userManager.IsInRoleAsync(user, "Author"))
            {
                _logger.LogWarn("Author already exists");
                return BadRequest("Author Already exists");
            }
            await _userManager.AddToRoleAsync(user, "Author");

            await _authorService.AddAuthor(user);
            return Ok();
        }

        [HttpGet]
        public IActionResult AllAuthors()
        {
            var ListOfUsernames = _authorService.GetAllAuthors();
            if (ListOfUsernames is null)
                return NoContent();
            return Ok(ListOfUsernames);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorById(Guid id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author is null)
                return NoContent();           
            
            await _authorService.SoftDeleteOrRetrieveAuthor(author);
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UndeleteAuthorById(Guid id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
                return NoContent();

            if (!author.IsDeleted)
            {
                _logger.LogInfo("Wrong call.Author still exists...");
                return BadRequest("Wrong call. Author still exists...");
            }            

            await _authorService.SoftDeleteOrRetrieveAuthor(author);
            return Ok();
        }
        
        [HttpGet("{id}", Name = "BookById")]
        public async Task<IActionResult> BookById(Guid id)
        {
            var book = await _bookService.GetBookByIDAsync(id);
            if (book == null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in our database.");
                return NotFound();
            }
            else            
                return Ok(book);            
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] AddBookDto bookDto)
        {
            if (bookDto == null)
            {
                _logger.LogError("You are not allowed to create a null entity");
                return NoContent();
            }
                       
            var author = await _authorService.GetAuthorByIdAsync(bookDto.AuthorId);
            if (author == null)
                return NoContent();

            var bookEntity = _mapper.Map<Book>(bookDto);
            await _bookService.AddBookAsync(bookEntity);

            var bookToReturnToClientView = _mapper.Map<DisplayBookDto>(bookEntity);
            return CreatedAtRoute("BookById", new { id = bookToReturnToClientView.Id }, bookToReturnToClientView);
        }                          
    }    
}