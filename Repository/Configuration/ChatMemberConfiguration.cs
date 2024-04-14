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
    public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> builder)
        {
            builder.ToTable("chat_member");
            builder.HasKey(t => new { t.AccountId, t.ChatId });
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(x => x.ChatId).HasColumnName("chat_id");
            builder.Property(t => t.ChatRole).HasColumnName("chat_role").IsRequired();
            builder.Property(pt => pt.JoinTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("join_time");
            
        }
    }
}
