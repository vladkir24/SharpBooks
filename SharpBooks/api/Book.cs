using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.api
{
    public class Book
    {
        public string BookName {get;set;}
            public string Author {get;set;}
               public string Edition {get;set;}
                    public string Publisher {get;set;}
                    public string Summary { get; set; }
                    public long Id { get; set; }
                    public int InStock { get; set; }
    }
}
