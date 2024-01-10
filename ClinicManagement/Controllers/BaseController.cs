using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Controllers
{
    //public class BaseController : Controller
    //{
    //    protected void ApplyPagination<T>(ref IEnumerable<T> items, int? page, int? entries, out int totalItems)
    //    {
    //        var pageSize = entries ?? 10; // Default to 10 entries per page
    //        var currentPage = page ?? 1; // Default to the first page

    //        totalItems = items.Count();

    //        items = items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
    //    }
    //}
    public class BaseController : Controller
    { 
        // ApplyPagination method for synchronous pagination
        protected (IEnumerable<T> Items, int TotalItems) ApplyPagination<T>(IEnumerable<T> items, int? page, int? entries)
        {
            // Default to 10 entries per page if entries is not specified
            var pageSize = entries ?? 5;
            // Default to the first page if page is not specified
            var currentPage = page ?? 1;
            // Calculate the total number of items in the collection
            var totalItems = items.Count();
            // Skip the items that precede the current page and take the specified number of items for the current page
            var paginatedItems = items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            // Return a tuple containing the paginated items and the total number of items
            return (paginatedItems, totalItems);
        }
     
    }

}
