using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SharpBooks.api;
using System.Net;
using System.IO;
using ServiceStack.Redis;

namespace SharpBooks.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ViewResult Index(int? page)
        {
            

            using (var redisClient = new RedisClient())
            {
                var pageIndex = page ?? 1;
                var pageSize = 20;
                var redisUsers = redisClient.As<Book>();
                var books = redisUsers.GetAll().OrderByDescending(a => a.Id).ToPagedList(pageIndex, pageSize);
                ViewBag.pageOfBooks = books;
                return View();
            }

            
        }


    }
}
