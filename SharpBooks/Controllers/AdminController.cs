using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using PagedList;
using SharpBooks.api;
using System.Globalization;
using System.Net;
using System.IO;
using ServiceStack.Redis;
using Newtonsoft.Json.Linq;

namespace SharpBooks.Controllers
{
    public class AdminController : Controller
    {
        private readonly static string getBookInfoUri = "http://isbndb.com/api/v2/json/KWC08NFB/book/";
        //http://isbndb.com/api/v2/json/[your-api-key]/book/9780849303159 
        // GET: /Admin/

        public ActionResult Index()
        {
            using (var redisClient = new RedisClient())
            {
                var redisUsers = redisClient.As<Book>();
                ViewBag.pageOfBooks = redisUsers.GetAll();
                return View();
            }
        }


        //[HttpPost]
        public ActionResult CreateFromId(string isbn)
        {
            string fullUri = getBookInfoUri + isbn;

            HttpWebRequest webRequest = GetWebRequest(fullUri);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            string jsonResponse = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = sr.ReadToEnd();
            }

            JObject o = JObject.Parse(jsonResponse);


            Book newBook = new Book();
            newBook.Id = long.Parse(isbn);
            newBook.BookName = (string)o["data"][0]["title"];
            newBook.Author = (string)o["data"][0]["author_data"][0]["name"];
            newBook.Edition = (string)o["data"][0]["edition_info"];
            newBook.Publisher = (string)o["data"][0]["publisher_text"];
            newBook.Summary = (string)o["data"][0]["summary"];

            using (var redisClient = new RedisClient())
            {
                var redisUsers = redisClient.As<Book>();
                ViewBag.pageOfBooks = redisUsers.GetAll();
                return View();
            }

            return View("Index");

           


            
        }

        [HttpPost]
        public ActionResult Create(Book bookInfo)
        {


            return RedirectToAction("Index");


        }

        private static HttpWebRequest GetWebRequest(string formattedUri)
        {
            // Create the request’s URI.
            Uri serviceUri = new Uri(formattedUri, UriKind.Absolute);

            // Return the HttpWebRequest.
            return (HttpWebRequest)System.Net.WebRequest.Create(serviceUri);
        }

    }
}
