﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class ChannelParameters : RequestParameters
    {
        public string? SearchTerm { get; set; }
        public string? Fields { get; set; }
    }
}
