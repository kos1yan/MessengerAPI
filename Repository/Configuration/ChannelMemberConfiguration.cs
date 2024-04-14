using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repository.Configuration
{
    public class ChannelMemberConfiguration : IEntityTypeConfiguration<ChannelMember>
    {
        public void Configure(EntityTypeBuilder<ChannelMember> builder)
        {
            builder.ToTable("channel_member");
            builder.HasKey(t => new { t.AccountId, t.ChannelId });
            builder.Property(x => x.ChannelId).HasColumnName("channel_id");
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(t => t.ChannelRole).HasColumnName("channel_role").IsRequired();
            builder.Property(pt => pt.JoinTime).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("join_time");
            



        }
    }
}
