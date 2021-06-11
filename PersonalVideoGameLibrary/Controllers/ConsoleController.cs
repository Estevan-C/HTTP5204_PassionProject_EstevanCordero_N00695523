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
    public class ConsoleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ConsoleController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Console/List
        public ActionResult List()
        {
            //To make a list of the console api and retrieve a list of consoles
            // curl https://localhost:44316/api/consoledata/listconsoles

            string url = "consoledata/listconsoles";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ConsoleDto> consoles = response.Content.ReadAsAsync<IEnumerable<ConsoleDto>>().Result;
            
            return View(consoles);
        }

        // GET: Console/Details/1
        // Will have to be updated to included a view model
        public ActionResult Details(int id)
        {
            DetailsConsole ViewModel = new DetailsConsole();

            // To retrieve one console from the list
            // curl https://localhost:44316/api/consoledata/findconsole/{id}

            string url = "consoledata/findconsole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ConsoleDto selectedConsole = response.Content.ReadAsAsync<ConsoleDto>().Result;
            
            ViewModel.SelectedConsole = selectedConsole;

            //Show all videogames assign to the console
            url = "videogamedata/listvideogamesforconsoles/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<VideoGameDto> assignVideoGames = response.Content.ReadAsAsync<IEnumerable<VideoGameDto>>().Result;

            ViewModel.AssignVideoGames = assignVideoGames;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Console/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Console/Create
        [HttpPost]
        public ActionResult Create(Consoles console)
        {

            // Add a new console into our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/consoledata/addconsole
            string url = "consoledata/addconsole";

            string jsonpayload = jss.Serialize(console);
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

        // GET: Console/Edit/1
        public ActionResult Edit(int id)
        {
            // To retrieve one console from the list
            // curl https://localhost:44316/api/consoledata/findconsole/{id}

            string url = "consoledata/findconsole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ConsoleDto selectedConsole = response.Content.ReadAsAsync<ConsoleDto>().Result;
            return View(selectedConsole);
        }

        // POST: Console/Update/1
        [HttpPost]
        public ActionResult Update(int id, Consoles console)
        {
            // Update a console into our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/consoledata/updateconsole/{id}

            string url = "consoledata/updateconsole/" + id;
            string jsonpayload = jss.Serialize(console);
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

        // GET: Console/DeleteConfirm/1
        public ActionResult DeleteConfirm(int id)
        {
            // To retrieve one console from the list
            // curl https://localhost:44316/api/consoledata/findconsole/{id}

            string url = "consoledata/findconsole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ConsoleDto selectedConsole = response.Content.ReadAsAsync<ConsoleDto>().Result;
            return View(selectedConsole);
        }

        // POST: Console/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Delete a console in our system
            // curl -H "Content-Type:application/json" -d @videogame.json https://localhost:44316/api/consoledata/deleteconsole

            string url = "consoledata/deleteconsole/" + id;
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
