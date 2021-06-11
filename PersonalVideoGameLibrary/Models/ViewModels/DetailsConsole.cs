using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalVideoGameLibrary.Models.ViewModels
{
    public class DetailsConsole
    {
        // Grabbing all information base on the console
        public ConsoleDto SelectedConsole { get; set; }

        // Grabing all videogame information assign to the console
        public IEnumerable<VideoGameDto> AssignVideoGames { get; set; }
    }
}