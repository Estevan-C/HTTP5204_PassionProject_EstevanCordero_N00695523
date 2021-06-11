using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalVideoGameLibrary.Models
{
    public class Consoles
    {
        // here we will have the id for conosles 
        // here we will the consoles name that will be used.
        [Key]
        public int ConsoleID { get; set; }
        public string ConsoleName { get; set; }

        // Will link with video games since conoles can have 
        // multiple video games and video games can have multiple consoles.
    
        // A consoles can have many video games
        public ICollection<VideoGames> VideoGames { get; set; }
    }

    // returning a simpler method know as a data transfer method

    public class ConsoleDto
    {
        public int ConsoleID { get; set; }
        public string ConsoleName { get; set; }
    }
}