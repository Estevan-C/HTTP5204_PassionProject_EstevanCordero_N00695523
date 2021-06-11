using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalVideoGameLibrary.Models.ViewModels
{
    public class UpdateVideoGame
    {
        // This view model will used to get the content of the videogame
        // and the content of the session message to updated

        // Existing videogame information
        public VideoGameDto SelectedVideoGame { get; set; }

        // All session msg to choose from
        public IEnumerable<SessionDto> SelectedMsg { get; set; }
    }
}