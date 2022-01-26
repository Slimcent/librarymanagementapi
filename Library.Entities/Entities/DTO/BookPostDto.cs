using System;

namespace Library.Entities.Entities.DTO
{
    public class BookPostDto
    {
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ISBN { get; set; }
        public string Category { get; set; }
        public Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
}
