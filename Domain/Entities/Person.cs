using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }
        public string Name { get; set; }

        public string ClientProfileID { get; set; }
        public virtual ClientProfile ClientProfile{ get; set; }

        public virtual ICollection<EventDate> EventDates { get; set; }

        public Person()
        {
            EventDates = new List<EventDate>();
        }
     }
}
