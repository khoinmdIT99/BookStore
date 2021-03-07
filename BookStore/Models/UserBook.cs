using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class UserBook
    {
        public int BookId { get; set; }
       // public int BookMovementId { get; set; }
        public string Name { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Count { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
    }
}