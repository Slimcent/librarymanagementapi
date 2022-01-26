using System;

namespace Library.Entities.Entities.Interfaces
{
    public interface ISoftDelete
    {
        DateTime? DeletedAt { get; set; }
        bool IsDeleted { get; set; }
    }
}
