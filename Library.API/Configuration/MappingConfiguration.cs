using AutoMapper;
using Library.Entities.Entities;
using Library.Entities.Entities.DTO;

namespace Library.API.Configuration
{
    public class MappingConfiguration : Profile
    {
       
        public MappingConfiguration()
        {
           

            CreateMap<User, DisplayUserDataDto>();
            CreateMap<Author, DisplayAuthorDto>()
                .ForMember(a => a.Username, opt => opt.MapFrom(b => b.User.UserName));         
                
            CreateMap<AddBookDto, Book>();
            CreateMap<Book, DisplayBookDto>();
            CreateMap<Book, BookGetDto>();
            CreateMap<BookPostDto, Book>();
            CreateMap<BookUpdateDto, Book>();
            CreateMap<RegisterModel, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        }
    }
}
