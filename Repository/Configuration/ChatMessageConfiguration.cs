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
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable("chat_message");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Text).HasMaxLength(2000).HasColumnName("text");
            builder.Property(x => x.Image).HasColumnName("image");
            builder.Property(x => x.PublicationTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("publication_time");
            builder.HasOne(x => x.Chat).WithMany(x => x.ChatMessages).HasForeignKey(x => x.ChatId);
            builder.Property(x => x.ChatId).HasColumnName("chat_id");
            builder.Property(x => x.AccountId).HasColumnName("account_id");
        }
    }
}
