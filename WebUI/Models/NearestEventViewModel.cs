using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class NearestEventViewModel
    {
        public Person Person { get; set; }
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }

    }
}
