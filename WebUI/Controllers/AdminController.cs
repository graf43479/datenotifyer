using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using UserStore.WEB.Filters;

namespace UserStore.WEB.Controllers
{
  //  [ServiceFilter(typeof(ExceptionLoggerFilter))]
  //[TypeFilter(typeof(ExceptionLoggerFilter))]
  //[ExceptionLoggerFilter()]
    public class AdminController : Controller
    {
        //IUnitOfWork Database { get; set; }
        UserManager<AppUser> _userManager;
        private ApplicationContext Database;

        //public AdminController(IUnitOfWork uow, UserManager<AppUser> userManager)
        //{
        //    Database = uow;
        //    _userManager = userManager;
        //}

        public AdminController(ApplicationContext context, UserManager<AppUser> userManager)
        {
            Database = context;
            _userManager = userManager;
        }

        //[ExceptionLoggerFilter(Database)]
        public async Task<ActionResult> Index()
        {
            var p = await _userManager.Users.ToListAsync(); // .Clients .GetUsersAsync();
            return View(p.ToList());
        }


        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
               Task<IdentityResult> result =  _userManager.DeleteAsync(user);
                if (result.Result.Succeeded)
                {                    
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Result.Errors);
                }
                
            }
            
                return View("Error", "Пользователь не найден!");
            
        }

        public async Task<ActionResult> ExceptionLogger()
        {
            IEnumerable<ExceptionDetail> exceptions = await Database.ExceptionDetails.ToListAsync();
            return View(exceptions);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteLogger(int? id)
        {
            if (id == null)
            {
                //OperationDetails result = await Database.ExceptionDetails.Remove .DeleteExceptionDetailAsync();
                IEnumerable<ExceptionDetail> details = await Database.ExceptionDetails.ToListAsync();
                Database.ExceptionDetails.RemoveRange(details);
            }
            else
            {
                ExceptionDetail result = await Database.ExceptionDetails.FirstOrDefaultAsync(x => x.Id == (int)id);
                if (result != null)
                {
                    Database.ExceptionDetails.Remove(result);
                }
            }
            Database.SaveChanges();
            return RedirectToAction("ExceptionLogger");
        }
    }
}