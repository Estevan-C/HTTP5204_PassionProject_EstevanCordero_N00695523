using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalVideoGameLibrary.Models
{
    public class Session
    {
        [Key]
        public int SessionID { get; set; }
        public string SessionMsg { get; set; }

        // Reference the VideoGame table
        // as a one to many relationship
        // A session can have multiple video games
        // A video game can only have one session
        [ForeignKey("VideoGames")]
        public int VideoGameID { get; set; }
        public virtual VideoGames VideoGames { get; set; }


    }


    // returning a simpler method know as a data transfer method
    public class SessionDto
    {
        public int SessionID { get; set; }
        public string SessionMsg { get; set; }

        public int VideoGameID { get; set; }

        public string VideoGameName { get; set; }
    }
}