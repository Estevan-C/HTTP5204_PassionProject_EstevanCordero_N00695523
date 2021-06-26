using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using PersonalVideoGameLibrary.Models;
using PersonalVideoGameLibrary.Models.ViewModels;
using System.Web.Script.Serialization;


namespace PersonalVideoGameLibrary.Controllers
{
    public class SessionController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SessionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }
        // GET: Session/List
        public ActionResult List()
        {
            //To make a list of the Session api and retrieve a list of sessions
            // curl https://localhost:44316/api/sessiondata/listsessions

            string url = "sessiondata/listsessions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<SessionDto> session = response.Content.ReadAsAsync<IEnumerable<SessionDto>>().Result;

            return View(session);
        }

        // GET: Session/Details/1
        public ActionResult Details(int id)
        {
            DetailsSession ViewModel = new DetailsSession();

            // Finds a session from the list
            // curl https://localhost:44316/api/sessiondata/findsessions/{id}

            string url = "sessiondata/findsession/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SessionDto selectedSession = response.Content.ReadAsAsync<SessionDto>().Result;

            ViewModel.SelectedSession = selectedSession;

            // Finds a video game that is connected to the message
            // curl https://localhost:44316/api/sessiondata/listsessionsforvideogames/{id}
            url = "sessiondata/listsessionsforvideogames/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<VideoGameDto> relatedVideoGame = response.Content.ReadAsAsync<IEnumerable<VideoGameDto>>().Result;
            ViewModel.RelatedVideoGame = relatedVideoGame;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Session/New
        public ActionResult New()
        {
            // Use to find the list of video games
            string url = "videogamedata/listvideogames";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<VideoGameDto> VideoGameOptions = response.Content.ReadAsAsync<IEnumerable<VideoGameDto>>().Result;
            
            return View(VideoGameOptions);
        }

        // POST: Session/Create
        [HttpPost]
        public ActionResult Create(Session session)
        {
            // Add a new session into our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/sessiondata/addsession

            string url = "sessiondata/addsession";

            string jsonpayload = jss.Serialize(session);
            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Session/Edit/1
        public ActionResult Edit(int id)
        {
            // Finds a session from the list
            // curl https://localhost:44316/api/sessiondata/findsessions/{id}

            string url = "sessiondata/findsession/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SessionDto SelectedSession = response.Content.ReadAsAsync<SessionDto>().Result;
            return View(SelectedSession);
        }

        // POST: Session/Edit/1
        [HttpPost]
        public ActionResult Update(int id, Session session)
        {
            // Update a new console into our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/sessiondata/updatesession

            string url = "sessiondata/updatesession/" + id;
            string jsonpayload = jss.Serialize(session);
            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Session/DeleteConfirm/1
        public ActionResult DeleteConfirm(int id)
        {
            // Finds a session from the list
            // curl https://localhost:44316/api/sessiondata/findsessions/{id}

            string url = "sessiondata/findsession/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SessionDto selectedSession = response.Content.ReadAsAsync<SessionDto>().Result;
            return View(selectedSession);
        }

        // POST: Session/Delete/1
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Delete a new console into our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/sessiondata/deletesession

            string url = "sessiondata/deletesession/" + id;
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
