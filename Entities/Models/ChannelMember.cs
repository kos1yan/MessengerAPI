using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ChannelMember
    {
       
        public DateTime JoinTime { get; set; }
        public string? ChannelRole {  get; set; }
        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        

    }
}
