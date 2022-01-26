using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities.Entities.DTO
{
    public class AddBookDto
    {
        [Required(ErrorMessage = "Input Id")]
        public Guid AuthorId { get; set; }
        [Required(ErrorMessage = "Input Title")]
    	[MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ISBN { get; set; }
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
