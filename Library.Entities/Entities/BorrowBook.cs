using Library.Entities.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Entities.Entities
{
    public class BorrowBook : ISoftDelete
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public DateTime BorrowedOn { get; set; }
        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
