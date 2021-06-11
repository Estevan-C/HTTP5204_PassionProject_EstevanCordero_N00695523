using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PersonalVideoGameLibrary.Models;
using PersonalVideoGameLibrary.Models.ViewModels;
using System.Web.Script.Serialization;


namespace PersonalVideoGameLibrary.Controllers
{
    public class VideoGameController : Controller
    {
        
        // Helps in reducing repetition in out code 
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // A method that will be used for the lifetime of our application.
        static VideoGameController()
        {
            client = new HttpClient();
            //Since the base address is the same we can create this line 
            // to help in reducing repetition
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: VideoGame/List
        public ActionResult List()
        {

            //To make a list with the video game api and retrieve a list of video games
            // curl https://localhost:44316/api/videogamedata/listvideogames

            string url = "videogamedata/listvideogames";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Output The response code is Ok
           // Debug.WriteLine("The response code is ");
           // Debug.WriteLine(response.StatusCode);

            // Parse content into a IEnumerable variable
            IEnumerable<VideoGameDto> videoGames = response.Content.ReadAsAsync<IEnumerable<VideoGameDto>>().Result;

            // Output Number of videogames: 4
           // Debug.WriteLine("Number of videogames: ");
           // Debug.WriteLine(videoGames.Count());

            return View(videoGames);
        }

        // GET: VideoGame/Details/10
        public ActionResult Details(int id)
        {
            // Included the view model DetailsVideoGame to list information of the console as
            // well as sessions that assign to that video game
            DetailsVideoGame ViewModel = new DetailsVideoGame();


            //To make call video game api and retrieve one videogame
            // curl https://localhost:44316/api/videogamedata/findvideogame/{id}

            string url = "videogamedata/findvideogame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Output The response code is Ok
           // Debug.WriteLine("The response code is ");
           // Debug.WriteLine(response.StatusCode);

            // Grabs one videogame.
            VideoGameDto selectedVideoGame = response.Content.ReadAsAsync<VideoGameDto>().Result;
            // Output Videogame received : 
            //Debug.WriteLine("Videogame received : ");
            //Debug.WriteLine(selectedVideoGame.VgName);

            // ---- Using the View model and calling consoles for videogames
            ViewModel.SelectedVideoGame = selectedVideoGame;

            // show associated consoles with the videogame
            url = "consoledata/listconsoleforvideogame/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ConsoleDto> AssingConsole = response.Content.ReadAsAsync<IEnumerable<ConsoleDto>>().Result;

            ViewModel.AssignConsole = AssingConsole;


            return View(ViewModel);
        }

        //POST: VideoGame/Assign/{videogameid}
        [HttpPost]
        public ActionResult Assign(int id, int consoleID)
        {
            Debug.WriteLine("Attempting to assign a videogame :" + id + " with the console " + consoleID);

            // call to the api to assign a video game with a console
            string url = "videogamedata/assignvideogamewithconsole/" + id + "/" + consoleID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //GET: VideoGame/UnAssign/{id}?ConsoleID={consoleID}
        [HttpGet]
        public ActionResult UnAssign(int id, int consoleID)
        {
            Debug.WriteLine("Attempting to unassign videogame :" + id + " with console: " + consoleID);

            //call to api to unassign a videogame with a console
            string url = "videogamedata/unassignvideogamewithconsole/" + id + "/" + consoleID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: VideoGame/New
        // This will be used to get the list of sessions with videogames
        // See Animal for example
        public ActionResult New()
        {
            //information about all sessions in the system
            // GET api/sessiondata/listsession

            string url = "sessiondata/listsession";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<SessionDto> sessionMsg = response.Content.ReadAsAsync<IEnumerable<SessionDto>>().Result;
            
            return View(sessionMsg);
        }

        // POST: VideoGame/Create
        [HttpPost]
        public ActionResult Create(VideoGames videogame)
        {
            // Tested to see if the method was called
            // Outputed the name of the videogame in the console
            Debug.WriteLine("Json payload is :");
            //Debug.WriteLine(videogame.VgName);

            // Add a new video game into our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/videogamedata/addvideogame
            string url = "videogamedata/addvideogame";

            
            string jsonpayload = jss.Serialize(videogame);

            // Accessing the full payload related to the videogame name
            Debug.WriteLine(jsonpayload);
            // All form input is passed and accessed

            // We need to convert the jsonpayload into a string for the request
            HttpContent content = new StringContent(jsonpayload);

            // We need to specify the header as seen when adding through json
            content.Headers.ContentType.MediaType = "application/json";

            // Test to see if there any problems with connecting to the url
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

            
        }

        // GET: VideoGame/Edit/10
        // This method will be used with a view model in order
        // to allow edit for videogame and session see example on how to make.
        public ActionResult Edit(int id)
        {
            UpdateVideoGame ViewModel = new UpdateVideoGame();

            // the existing information for videogame
            string url = "videogamedata/findvideogame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VideoGameDto selectedVideoGame = response.Content.ReadAsAsync<VideoGameDto>().Result;
            
            ViewModel.SelectedVideoGame = selectedVideoGame;

            // All sessionMsg to choose from when updating this videogame
            url = "sessiondata/listsessions/";
            response = client.GetAsync(url).Result;
            IEnumerable<SessionDto> selectedMsg = response.Content.ReadAsAsync<IEnumerable<SessionDto>>().Result;

            ViewModel.SelectedMsg = selectedMsg;

            return View(ViewModel);
        }

        // POST: VideoGame/Update/10
        [HttpPost]
        public ActionResult Update(int id, VideoGames videogame)
        {
            // Update a video game in the system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/videogamedata/updatevideogame

            string url = "videogamedata/updatevideogame/"+id;
            string jsonpayload = jss.Serialize(videogame);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // Making a delete confirm method, so that users can back out
        // if they change there mind
        // GET: VideoGame/Delete/10
        public ActionResult DeleteConfirm(int id)
        {
            string url = "videogamedata/deletevideogame" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VideoGameDto selectedvideogame = response.Content.ReadAsAsync<VideoGameDto>().Result;
            return View(selectedvideogame);
        }


        // POST: VideoGame/Delete/10
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Delete a video game in our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/videogamedata/deletevideogame

            string url = "videogamedata/deletevideogame/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
