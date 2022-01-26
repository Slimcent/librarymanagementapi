using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Entities.Entities.DTO
{
    public class BookBorrowGetDto
    {
        public int Id { get; set; }
        public DateTime BorrowedOn { get; set; }
        public DateTime DueDate { get; set; }
    }
}
