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
    public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.ToTable("channel");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Description).HasMaxLength(200).HasColumnName("description");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
            builder.Property(x => x.Image).HasColumnName("image");
            builder.Property(x => x.ConnectionId).IsRequired().HasColumnName("connection_id");
            builder.Property(x => x.CreationTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("creation_time");
            builder
            .HasMany(e => e.Accounts)
            .WithMany(e => e.Channels)
            .UsingEntity<ChannelMember>(
             r => r.HasOne<Account>(e => e.Account).WithMany(e => e.ChannelMembers).HasForeignKey(e => e.AccountId),
             l => l.HasOne<Channel>(e => e.Channel).WithMany(e => e.ChannelMembers).HasForeignKey(e => e.ChannelId));



        }
    }
}
