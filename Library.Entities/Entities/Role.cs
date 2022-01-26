using Library.Entities.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace Library.Entities.Entities
{
    public class Role : IdentityRole, ISoftDelete
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set ; }
        public bool IsDeleted { get ; set ; }
    }
}
