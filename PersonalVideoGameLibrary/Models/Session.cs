using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PersonalVideoGameLibrary.Models
{
    public class Session
    {
        [Key]
        public int SessionID { get; set; }
        public string SessionMsg { get; set; }


    }

    // returning a simpler method know as a data transfer method
    public class SessionDto
    {
        public int SessionID { get; set; }
        public string SessionMsg { get; set; }
    }
}