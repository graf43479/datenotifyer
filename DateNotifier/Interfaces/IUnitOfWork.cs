using DateNotifier.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DateNotifier.Interfaces
{
    public interface IUnitOfWork
    {

        IRepository<Person> Persons { get; }
        IRepository<EventType> EventTypes { get; }
        IRepository<EventDate> EventDates { get; }
        IRepository<ExceptionDetail> ExceptionDetails { get; }
        Task SaveAsync();
    }
}
