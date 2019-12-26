using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("AppUser")]
        public string ClientProfileID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

        public ClientProfile()
        {
            Persons = new List<Person>();
        }
    }
}
