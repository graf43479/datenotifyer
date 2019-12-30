using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class EventDateViewModel
    {
        public int EventDateID { get; set; }
        public int PersonID { get; set; }

        public int EventTypeID { get; set; }

        public DateTime Date { get; set; }

        public bool IsEnabled { get; set; }
        public int Prirority { get; set; }
        public int SelectedEventTypeID { get; set; }
        public ICollection<EventType> EventTypes { get; set; }
    }
}
