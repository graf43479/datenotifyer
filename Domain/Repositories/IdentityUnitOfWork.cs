using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ClientRepository clientRepository;
        private ExceptionRepository exceptionRepository;
        private EventDateRepository eventDateRepository;
        private EventTypeRepository eventTypeRepository;
        private PersonRepository personRepository;

        public IdentityUnitOfWork(DbContextOptions<ApplicationContext> options)
        {            
            db = new ApplicationContext(options);            
            clientRepository = new ClientRepository(db);
            exceptionRepository = new ExceptionRepository(db);
            eventDateRepository = new EventDateRepository(db);
            eventTypeRepository = new EventTypeRepository(db);
            personRepository = new PersonRepository(db);
        }
       
        public IRepository<ClientProfile> Clients
        {
            get
            {
                if (clientRepository == null)
                    clientRepository = new ClientRepository(db);
                return clientRepository;
            }
        }

        public IRepository<Person> Persons
        {
            get
            {
                if (personRepository == null)
                    personRepository  = new PersonRepository(db);
                return personRepository;
            }
        }

        public IRepository<ExceptionDetail> ExceptionDetails
        {
            get
            {
                if (exceptionRepository == null)
                    exceptionRepository = new ExceptionRepository(db);
                return exceptionRepository;
            }
        }

        public IRepository<EventType> EventTypes
        {
            get
            {
                if (eventTypeRepository == null)
                    eventTypeRepository = new EventTypeRepository(db);
                return eventTypeRepository;
            }
        }

        public IRepository<EventDate> EventDates
        {
            get
            {
                if (eventDateRepository == null)
                    eventDateRepository = new EventDateRepository(db);
                return eventDateRepository;
            }
        }
      
        public void Dispose()
        {
            db.Dispose();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }     
    }
}
