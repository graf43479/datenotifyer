using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private IdentityUnitOfWork db;
        public PersonController(IdentityUnitOfWork context)
        {
            db = context;
        }

        public IActionResult Index()
        {            
            var types = db.Persons.GetAll();
            return View(types);
        }

        public async Task<IActionResult> Create()
        {
            AppUser user = await db.UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return View("Edit", new Person() { ClientProfileID = user.Id });            
        }        

        public IActionResult Edit(int personID)
        {
            Person person = db.Persons.Get(personID);
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                if (person.PersonID <= 0)
                {
                    db.Persons.Create(person);
                    await db.SaveAsync();
                }
                else
                {
                    db.Persons.Update(person);
                    await db.SaveAsync();
                }
            }
            else
            {
                return View(person);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int personID)
        {
            Person person = db.Persons.Get(personID);
            db.Persons.Delete(person);
            await db.SaveAsync();
            return RedirectToAction("Index");
        }


        public IActionResult PersonEvents(int personID)
        {
            PersonEventsViewModel viewModel = new PersonEventsViewModel()
            {
                EventDates = db.EventDates.GetAll().Where(x => x.PersonID == personID).ToList(),
                Person = db.Persons.Get(personID)
            };
            
            return PartialView(viewModel);
        }

        public IActionResult CreateDate(int personID)
        {            
            return View("EditDate", new EventDateViewModel() { EventTypes = db.EventTypes.GetAll().ToList(), PersonID=personID });
        }

        public IActionResult EditDate(int eventdateID)
        {
            EventDate date = db.EventDates.Get(eventdateID);

            return View(new EventDateViewModel() 
            {
                   EventDateID = date.EventDateID,
                   Date = date.Date,
                   EventTypeID = date.EventTypeID,
                   IsEnabled = date.IsEnabled,
                   Prirority = date.Prirority,
                   PersonID = date.PersonID,
                   //SelectedEventTypeID = "",
                   EventTypes = db.EventTypes.GetAll().ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDate(EventDateViewModel dateModel)
        {
            EventDate date = new EventDate()
            {
                EventDateID = dateModel.EventDateID,
                Date = dateModel.Date,
                EventTypeID = dateModel.SelectedEventTypeID,
                IsEnabled = dateModel.IsEnabled,
                Prirority = dateModel.Prirority,
                PersonID = dateModel.PersonID                
            };

            if (ModelState.IsValid)
            {
                if (date.EventDateID <= 0)
                {
                    db.EventDates.Create(date);
                    await db.SaveAsync();
                }
                else
                {
                    db.EventDates.Update(date);
                    await db.SaveAsync();
                }
            }
            else
            {
                return View(date);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteDate(int eventdateId)
        {
            EventDate date = db.EventDates.Get(eventdateId);
            db.EventDates.Delete(date);
            await db.SaveAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> NearestEventsList()
        {
            AppUser user = await GetCurrentUserAsync();
            //var persons = db.Persons.GetAll().Where(x => x.ClientProfileID == user.Id);
            int dayNum = DateTime.Now.DayOfYear;

            //TODO: коррекция ближайших дат в случае нового года
            var viewModel = from p in db.Persons.GetAll()
                         join ev in db.EventDates.GetAll() on p.PersonID equals ev.PersonID
                         where p.ClientProfileID == user.Id
                         && ev.Date.DayOfYear > dayNum
                         orderby ev.Date.DayOfYear
                         select new NearestEventViewModel()
                         {
                             Date = ev.Date,
                             Person = p,
                             EventType = ev.EventType
                         };


            return View(viewModel.Take(10));

        }

        private async Task<AppUser> GetCurrentUserAsync()
        {
            AppUser user = await db.UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return user;
        }

    }
}