using Library.Entities.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities.Entities
{
    public class Author : ISoftDelete
    {     
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool LockoutEnabled { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
