using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("contact");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.FriendContactId).IsRequired().HasColumnName("friend_contact_id");
            builder.Property(x => x.AddTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("add_time");
            builder.HasOne(u => u.Account).WithMany(c => c.Contacts).HasForeignKey(u => u.AccountId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.AccountId).HasColumnName("account_id");



        }
    }
}
