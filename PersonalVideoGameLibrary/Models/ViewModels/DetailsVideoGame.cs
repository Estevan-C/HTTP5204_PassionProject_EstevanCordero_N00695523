using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalVideoGameLibrary.Models.ViewModels
{
    public class DetailsVideoGame
    {
        // References the VideoGameDto in the VideoGames Model
        public VideoGameDto SelectedVideoGame { get; set; }

        // Consoles that are already assign to a video game
        public IEnumerable<ConsoleDto> AssignConsole { get; set; }

        // Sessions that are assign to a video game
        //public IEnumerable<SessionDto> AssignSession { get; set; }

    }
}