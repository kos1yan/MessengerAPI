using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Channel
    {
        public Guid Id { get; set; }

        public string? Image {  get; set; }
        public string Name { get; set; }
        public string? Description {  get; set; }

        public Guid ConnectionId { get; set; }

        public DateTime CreationTime { get; set; }

        public List<ChannelMember> ChannelMembers { get; } = new();
        public List<Account> Accounts { get; }
        public List<ChannelMessage>? ChannelMessages {  get; set; }


    }
}
