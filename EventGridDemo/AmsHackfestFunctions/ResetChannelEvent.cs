using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PusherEventGrid
{
    public class ResetChannelEvent
    {
        public string ChannelPID { get; set; }
        public string ChannelName { get; set; }
        public string AmsEdpoint { get; set; }
    }
}
