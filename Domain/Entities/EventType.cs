using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }
        public string EventName { get; set; }


        public virtual ICollection<EventDate> EventDates { get; set; }

        public EventType()
        {
            EventDates = new List<EventDate>();
        }

    }
}
