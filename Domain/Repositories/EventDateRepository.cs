using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Repositories
{
    public class EventDateRepository : IRepository<EventDate>
    {
        private ApplicationContext db;
        public EventDateRepository(ApplicationContext context)
        {
            db = context;
        }
        public void Create(EventDate eventType)
        {
            db.EventDates.Add(eventType);
        }

        public void Update(EventDate eventType)
        {
            db.Entry(eventType).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            EventDate ex = db.EventDates.Find(id);
            if (ex != null)
                db.EventDates.Remove(ex);
        }

        public void Delete(EventDate eventType)
        {
            db.EventDates.Remove(eventType);
        }

        public IEnumerable<EventDate> Find(Func<EventDate, Boolean> predicate)
        {
            return db.EventDates.Where(predicate).ToList();
        }

        public IEnumerable<EventDate> GetAll()
        {
            return db.EventDates;
        }

        public EventDate Get(int id)
        {
            return db.EventDates.Find(id);
        }
    }
}
