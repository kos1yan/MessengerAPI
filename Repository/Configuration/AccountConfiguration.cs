using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Repository.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Bio).HasMaxLength(200).HasColumnName("bio");
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(50).HasColumnName("name");
            builder.Property(x => x.Image).HasColumnName("image");
            builder.HasOne(u => u.User).WithOne(c => c.Account).HasForeignKey<User>(c => c.AccountId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
