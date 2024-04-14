using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelMemberConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelMessageConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMemberConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Chat>? Chats { get; set; }
        public DbSet<ChatMessage>? Messages { get; set; }
        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<ChatMember>? ChatMembers { get; set; }
        public DbSet<Channel>? Channels { get; set; }
        public DbSet<ChannelMember>? ChannelMembers { get; set; }
        public DbSet<ChannelMessage>? ChannelMessages { get; set; }
    }
}
