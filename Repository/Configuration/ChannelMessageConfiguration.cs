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
    public class ChannelMessageConfiguration : IEntityTypeConfiguration<ChannelMessage>
    {
        public void Configure(EntityTypeBuilder<ChannelMessage> builder)
        {
            builder.ToTable("channel_message");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Text).HasMaxLength(2000).HasColumnName("text");
            builder.Property(x => x.Image).HasColumnName("image");
            builder.Property(x => x.PublicationTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("publication_time");
            builder.HasOne(x => x.Channel).WithMany(x => x.ChannelMessages).HasForeignKey(x => x.ChannelId);
            builder.Property(x => x.ChannelId).HasColumnName("channel_id");
        }
    }
}
