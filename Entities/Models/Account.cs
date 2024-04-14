using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace Entities.Models
{
    public class Account 
    {
        public Guid Id { get; set; }
        public string? Bio {  get; set; }
        public string UserName {  get; set; }
        public string? Image { get; set; }
        public List<ChatMember>? ChatMembers { get; set; } = new();
        public List<Chat>? Chats { get; set; }
        public List<ChannelMember> ChannelMembers { get; }
        public List<Channel> Channels { get; }
        public List<Contact>? Contacts { get; set; }
        public User? User { get; set; }
    }
}
