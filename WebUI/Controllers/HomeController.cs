using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserStore.WEB.Models;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Domain;

namespace UserStore.WEB.Controllers
{
    //  [ServiceFilter(typeof(ExceptionLoggerFilter))]
    
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
             db = context;
            InitData();
            
        }

        public IActionResult Index()
        {
          //  var p = _roleManager.Roles.ToList();
            return View() ;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void InitData()
        {
            if (!db.EventTypes.Any())
            {
                List<EventType> eventTypes = new List<EventType>
                {
                    new EventType(){ EventName = "День рождения"},
                    new EventType(){ EventName = "Годовщина свадьбы"},
                    new EventType(){ EventName = "День учителя"}
                };
                db.EventTypes.AddRange(eventTypes);
                db.SaveChanges();
            }

            //if (!db.Persons.Any())
            //{
            //    List<Person> people = new List<Person>()
            //    {
            //        new Person{ Name = "Ivanov", ClientProfile = db.Persons.First().ClientProfile},
            //        new Person{ Name = "Petrov",ClientProfile = db.Persons.First().ClientProfile},
            //        new Person{ Name = "Sidorov", ClientProfile = db.Persons.First().ClientProfile}
            //    };
            //    db.Persons.AddRange(people);
            //    db.SaveChanges();
            //}
        }
    }
}
