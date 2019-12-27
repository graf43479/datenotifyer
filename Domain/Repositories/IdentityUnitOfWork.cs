using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

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

        //private AppUserManager userManager;
        //private AppRoleManager roleManager;
        //private ILogger<AccountController> _logger;
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        //private SignInManager<AppUser> _signInManager;


        public IdentityUnitOfWork(DbContextOptions<ApplicationContext> options, UserManager<AppUser> uManager)
        {            
            db = new ApplicationContext(options);            
            clientRepository = new ClientRepository(db);
            exceptionRepository = new ExceptionRepository(db);
            eventDateRepository = new EventDateRepository(db);
            eventTypeRepository = new EventTypeRepository(db);
            personRepository = new PersonRepository(db);

            userManager = uManager; //new AppUserManager(new UserStore<AppUser>(db));
            //userManager = new AppUserManager(new UserStore<AppUser>(db));
            // roleManager = new AppRoleManager(new RoleStore<AppRole>(db));
        }

        public UserManager<AppUser> UserManager => userManager;

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
