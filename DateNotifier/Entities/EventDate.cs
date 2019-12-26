using System;
using System.Collections.Generic;
using System.Text;

namespace DateNotifier.Entities
{
    public  class EventDate
    {
        public int DateEventID { get; set; }
        public int PersonID { get; set; }

        public bool IsEnabled { get; set; }
        public int Prirority { get; set; }
        public virtual Person Person { get; set; }
        public virtual EventType EventType { get; set; }

    }
}
