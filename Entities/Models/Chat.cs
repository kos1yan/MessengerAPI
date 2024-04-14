using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        
        public string? Name { get; set; }

        public string? Image { get; set; }
        public DateTime StartTime { get; set; }

        public Guid ConnectionId {  get; set; }

        public List<ChatMessage>? ChatMessages { get; set; }

        public List<ChatMember> ChatMembers { get; set; } = new();
        public List<Account> Accounts { get; set; }

    }
}
