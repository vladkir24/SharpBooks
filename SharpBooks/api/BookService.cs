using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;

namespace SharpBooks.api
{
    public class BookService : Service
    {
        public IRepository Repository { get; set; }

        public object Post(AddBook request)
        {
            var id = Repository.AddBook(request.ISBN, request.BookName, request.Author, request.Edition, request.Publisher, request.Summary);
            return new AddBookResponse { ISBN = id };
        }

        public object Get(Books request)
        {
            return new BooksResponse{ books = Repository.GetBooks()};
        }

    }



    public class BooksResponse
    {
        public IEnumerable<Book> books { get; set; }
    }

    [Route("/books", "GET")]
    public class Books
    {
    }



    [Route("/books", "POST")]
    public class AddBook
    {
        public long ISBN { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string Summary { get; set; }
        public int InStock { get; set; }
    }


    public class AddBookResponse
    {
        public long ISBN { get; set; }
    }
}