using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalVideoGameLibrary.Models
{
    public class VideoGames
    {
        [Key]
        public int VideoGameID { get; set; }
        public string VgName { get; set; }
        public decimal VgPrice { get; set; }
        public int VgHours { get; set; }

       

        // Reference the session table
        // as a one to many relationship
        // A video game can have multiple sessions
        // A session can only have one video game
        [ForeignKey("Session")]
        public int SessionID { get; set; }
        public virtual Session Session { get; set; }

        // A video game can belong to many consoles
        public ICollection<Consoles> Consoles { get; set; }



    }

    // returning a simpler method know as a data transfer method

    public class VideoGameDto
    {
        public int VideoGameID { get; set; }
        public string VgName { get; set; }
        public decimal VgPrice { get; set; }
        public int VgHours { get; set; }
        public int SessionID { get; set; }
        public string SessionMsg { get; set; }
    }
}