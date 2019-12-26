using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace UserStore.WEB.Filters
{
    public class ExceptionLoggerFilter : Attribute, IExceptionFilter
    {
        // IUnitOfWork Database { get; set; }
        ApplicationContext Database { get; set; }

        //public ExceptionLoggerFilter(IUnitOfWork uow)
        //{
        //    Database = uow;
        //}
        public ExceptionLoggerFilter(ApplicationContext context)
        {
            Database = context;
        }

        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetail exceptionDetail = new ExceptionDetail()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };

            //Database.ExceptionDetails.Create(exceptionDetail);
            Database.ExceptionDetails.Add(exceptionDetail); // Create(exceptionDetail);
            Database.SaveChanges();

            filterContext.Result = new ViewResult { ViewName = "Error" }; // RedirectResult("Shared/Error");
            filterContext.ExceptionHandled = true;
        }
    }
}
