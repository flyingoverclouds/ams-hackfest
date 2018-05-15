using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PusherEventGrid
{
    public class CustomEvent<T>
    {
        public string Id { get; private set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        public string EventTime { get; private set; }
        public T Data { get; set; }
        public CustomEvent()
        {
            Id = Guid.NewGuid().ToString();

            DateTime localTime = DateTime.Now;
            DateTime utcTime = DateTime.UtcNow;
            DateTimeOffset localTimeAndOffset =
                new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));
            EventTime = localTimeAndOffset.ToString("o");
        }
    }
}
