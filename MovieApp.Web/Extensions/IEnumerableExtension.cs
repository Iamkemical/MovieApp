using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApp.Web.Models;

namespace MovieApp.Web.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            };
            List.Add(selectList);
            foreach (var item in Items)
            {
                selectList = new SelectListItem
                {
                    Text = item.GetPropertyValue("Name"),
                    Value = item.GetPropertyValue("Id")
                };
            List.Add(selectList);
            }
            return List;
        }
    }
}
