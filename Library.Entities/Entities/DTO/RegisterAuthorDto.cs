using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities.Entities.DTO
{
    public class RegisterAuthorDto
    {
        [Required(ErrorMessage = "Ensure you input User name")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Username { get; set; }
    }
}
