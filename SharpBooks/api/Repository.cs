using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace SharpBooks.api
{

    public interface IRepository
    {

        long AddBook(long ISBN, string BookName, string Author, string Edition, string Publisher, string Summary);
        IEnumerable<Book> GetBooks();
        Book GetBooks(long isbn);
        void UpdateStock(Book book);

    }

    public class Repository : IRepository
    {
        IRedisClientsManager RedisManager { get; set; }

        public Repository(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        public IEnumerable<Book> GetBooks()
        {
            using (var redisClient = RedisManager.GetClient())
            {
                var redisUsers = redisClient.As<Book>();
                return redisUsers.GetAll();
            }
        }

        public Book GetBooks(long isbn)
        {
            using (var redisClient = RedisManager.GetClient())
            {
                var redisUsers = redisClient.As<Book>();
                return redisUsers.GetById(isbn);
            }
        }


        public long AddBook(long isbn, string bookName, string author, string edition, string publisher, string summary)
        {
            using (var redisClient = RedisManager.GetClient())
            {
                var redisUsers = redisClient.As<Book>();


                if(redisUsers.GetById(isbn) !=null)
                {
                    var book = GetBooks(isbn);
                    book.InStock++;
                    UpdateStock(book);
                    return isbn;
                }
                else
                {
                    var book = new Book() { Id = isbn, BookName = bookName, Author = author, Edition = edition, Publisher = publisher, Summary = summary, InStock = 1 };
                    redisUsers.Store(book);
                    return isbn;
                }

            }
        }

        public void UpdateStock(Book book)
        {
            using (var redisClient = RedisManager.GetClient())
            {
                var redisUsers = redisClient.As<Book>();
                redisUsers.Store(book);
            };
        }
    }

    

}