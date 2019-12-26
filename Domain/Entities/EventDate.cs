using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public  class EventDate
    {
        [Key]
        public int EventDateID { get; set; }
        public int PersonID { get; set; }

        public int EventTypeID { get; set; }

        public DateTime Date { get; set; }

        public bool IsEnabled { get; set; }
        public int Prirority { get; set; }
        public virtual Person Person { get; set; }
        public virtual EventType EventType { get; set; }

    }
}
