using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private ApplicationContext db;
        public PersonRepository(ApplicationContext context)
        {
            db = context;
        }
        public void Create(Person person)
        {
            db.Persons.Add(person);
        }

        public void Update(Person person)
        {
            db.Entry(person).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            Person ex = db.Persons.Find(id);
            if (ex != null)
                db.Persons.Remove(ex);
        }

        public void Delete(Person person)
        {
            db.Persons.Remove(person);
        }

        public IEnumerable<Person> Find(Func<Person, Boolean> predicate)
        {
            return db.Persons.Where(predicate).ToList();
        }

        public IEnumerable<Person> GetAll()
        {
            return db.Persons;
        }

        public Person Get(int id)
        {
            return db.Persons.Find(id);
        }
    }
}
