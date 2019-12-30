using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI
{
    public static class HtmlHelpers
    {
        public static IEnumerable<SelectListItem> GetAllEventTypes(ICollection<EventType> eventTypes)
        {
            //var categories = category.GetAll();
            //return categories.Select(x => new SelectListItem { Text = x.categoryName, Value = x.categoryId.ToString() }));
            return eventTypes.Select(x => new SelectListItem { Text = x.EventName, Value = x.EventTypeID.ToString() });
        }
    }
}
