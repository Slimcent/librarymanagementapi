using Library.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Library.Entities.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData
             (             
             new Book
             {
                 Id = new Guid("33ba8148-7cb0-41c8-94b6-578186f8e46f"),
                 Title = "Purple Rose",
                 PublishedOn = DateTime.Now,
                 ISBN = 2098,
                 Category = "Fiction",
                 AuthorId = new Guid("6cc69bd3-8a05-4690-bae3-85cc5af6db34"),
             },
             new Book
             {
                 Id = new Guid("8b6d96a3-4344-4012-87d4-2a791c4b7c96"),
                 Title = "Green Rose",
                 PublishedOn = DateTime.Now,
                 ISBN = 2097,
                 Category = "Finance",
                 AuthorId = new Guid("6cc69bd3-8a05-4690-bae3-85cc5af6db34"),
             });
        }
        
    }
}
