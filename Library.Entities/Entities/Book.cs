using Library.Entities.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities.Entities
{
    public class Book : ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ISBN { get; set; }
        public string Category { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
