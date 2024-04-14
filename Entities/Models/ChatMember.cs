using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ChatMember
    {
        public DateTime JoinTime { get; set; }
        public string? ChatRole { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}
