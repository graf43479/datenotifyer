using DateNotifier.Entities;
using DateNotifier.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateNotifier.Repositories
{
   public class ClientRepository : IRepository<ClientProfile>
    {
        private ApplicationContext db;
        public ClientRepository(ApplicationContext context)
        {
            db = context;
        }
        public void Create(ClientProfile client)
        {
            db.ClientProfiles.Add(client);
        }

        public void Update(ClientProfile client)
        {
            db.Entry(client).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            ClientProfile ex = db.ClientProfiles.Find(id);
            if (ex != null)
                db.ClientProfiles.Remove(ex);
        }

        public void Delete(ClientProfile client)
        {
            db.ClientProfiles.Remove(client);
        }

        public IEnumerable<ClientProfile> Find(Func<ClientProfile, Boolean> predicate)
        {
            return db.ClientProfiles.Where(predicate).ToList();
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return db.ClientProfiles;
        }

        public ClientProfile Get(int id)
        {
            return db.ClientProfiles.Find(id);
        }
    }
}
