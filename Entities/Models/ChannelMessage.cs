﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ChannelMessage
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public DateTime PublicationTime { get; set; }
        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
