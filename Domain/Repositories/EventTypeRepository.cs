using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Repositories
{
    public class EventTypeRepository : IRepository<EventType>
    {
        private ApplicationContext db;
        public EventTypeRepository(ApplicationContext context)
        {
            db = context;
        }
        public void Create(EventType eventType)
        {
            db.EventTypes.Add(eventType);
        }

        public void Update(EventType eventType)
        {
            db.Entry(eventType).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            EventType ex = db.EventTypes.Find(id);
            if (ex != null)
                db.EventTypes.Remove(ex);
        }

        public void Delete(EventType eventType)
        {
            db.EventTypes.Remove(eventType);
        }

        public IEnumerable<EventType> Find(Func<EventType, Boolean> predicate)
        {
            return db.EventTypes.Where(predicate).ToList();
        }

        public IEnumerable<EventType> GetAll()
        {
            return db.EventTypes;
        }

        public EventType Get(int id)
        {
            return db.EventTypes.Find(id);
        }
    }
}
