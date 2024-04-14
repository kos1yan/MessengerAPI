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
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("chat");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
            builder.Property(x => x.Image).HasColumnName("image");
            builder.Property(x => x.ConnectionId).IsRequired().HasColumnName("connection_id");
            builder.Property(x => x.StartTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("start_time");
            builder
           .HasMany(e => e.Accounts)
           .WithMany(e => e.Chats)
           .UsingEntity<ChatMember>(
            r => r.HasOne<Account>(e => e.Account).WithMany(e => e.ChatMembers).HasForeignKey(e => e.AccountId),
            l => l.HasOne<Chat>(e => e.Chat).WithMany(e => e.ChatMembers).HasForeignKey(e => e.ChatId));
        }
    }
}
