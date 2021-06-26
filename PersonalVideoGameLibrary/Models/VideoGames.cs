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
        public string VideoGameName { get; set; }
        public decimal VideoGamePrice { get; set; }
        public int VideoGameHours { get; set; }

        // New Feature to allow images to be included with 
        // the current information of video games.

        // Data needed to keep track of videogame images uploaded
        // Images can be found into /Content/Images/VideoGames/{id}.{extension}
        public bool VideoGameHasPic { get; set; }
        public string PicExtension { get; set; }

        // A video game can belong to many consoles
        public ICollection<Consoles> Consoles { get; set; }



    }

    // returning a simpler method know as a data transfer method

    public class VideoGameDto
    {
        public int VideoGameID { get; set; }
        public string VideoGameName { get; set; }
        public decimal VideoGamePrice { get; set; }
        public int VideoGameHours { get; set; }
        
        //public int SessionID { get; set; }
        //public string SessionMsg { get; set; }
        
        public bool VideoGameHasPic { get; set; }
        public string PicExtension { get; set; }

    }
}