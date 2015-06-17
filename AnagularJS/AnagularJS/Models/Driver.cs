using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnagularJS.Models
{
    public class Driver
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object BirthDate { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}