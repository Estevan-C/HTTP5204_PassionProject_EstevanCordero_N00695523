using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalVideoGameLibrary.Models.ViewModels
{
    public class DetailsSession
    {
        //Grabs the sesssion and its content
        public SessionDto SelectedSession { get; set; }

        //Grabs the videogame and its content
        public IEnumerable<VideoGameDto> RelatedVideoGame { get; set; }
    }
}