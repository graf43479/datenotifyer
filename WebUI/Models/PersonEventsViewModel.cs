using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class PersonEventsViewModel
    {
        public IEnumerable<EventDate> EventDates { get; set; }
        public  Person Person { get; set; }
    }
}
