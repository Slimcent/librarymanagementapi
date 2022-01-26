using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Entities.Entities.DTO
{
    public class BookBorrowPostDto
    {
        public Guid AuthorId { get; set; }
        public DateTime BorrowedOn { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
