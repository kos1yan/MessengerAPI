using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Contact
    {
        public Guid Id { get; set; }

        public Guid FriendContactId {  get; set; }
        public DateTime AddTime { get; set; }
        public Guid AccountId { get; set; }

        public Account Account { get; set; }




    }
}
