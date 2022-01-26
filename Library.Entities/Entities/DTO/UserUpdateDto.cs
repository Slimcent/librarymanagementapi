using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities.Entities.DTO
{
    public class UserUpdateDto
    {
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string FirstName { get; set; }
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string LastName { get; set; }
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string UserName { get; set; }              
        public string PhoneNumber { get; set; }
        [ProtectedPersonalData]
        [EmailAddress]
        public virtual string Email { get; set; }     
              
    }
}
