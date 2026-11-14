using System;
using System.Collections.Generic;
using System.Text;

namespace EF_CORE2
{
    public class Badge
    {
        public int AttendeeId { get; set; }    
        public int BadgeNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public BadgeTier Tier { get; set; }

        public Attendee Attendee { get; set; } = null!;
    }
}
