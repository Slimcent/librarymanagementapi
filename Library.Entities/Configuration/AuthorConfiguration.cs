using Library.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Library.Entities.Configuration
{

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData
             (
             new Author
             {
                 Id = new Guid("6cc69bd3-8a05-4690-bae3-85cc5af6db34"),
                 CreatedAt = DateTime.Now, 
                 
             });
        }
    }
}
