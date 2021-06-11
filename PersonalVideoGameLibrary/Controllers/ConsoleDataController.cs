using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PersonalVideoGameLibrary.Models;

namespace PersonalVideoGameLibrary.Controllers
{
    public class ConsoleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List of all consoles in the system
        /// </summary>
        /// <returns>
        /// List all console in the system and name for it.
        /// </returns>
        /// <example>
        // GET: api/ConsoleData/ListConsoles
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ConsoleDto))]
        public IHttpActionResult ListConsoles()
        {
            List<Consoles> Consoles = db.Consoles.ToList();
            List<ConsoleDto> ConsoleDtos = new List<ConsoleDto>();

            Consoles.ForEach(c => ConsoleDtos.Add(new ConsoleDto()
            {
                ConsoleID = c.ConsoleID,
                ConsoleName = c.ConsoleName

            }));

            return Ok(ConsoleDtos);
        }

        /// <summary>
        /// Returns all consoles in the system associated with a particular videogame
        /// </summary>
        /// <param name="id">Videogame Primary Key</param>
        /// <returns>
        /// 200 Ok : All console in the database assign to the videogame
        /// </returns>
        /// <example>
        /// GET: api/ConsoleData/ListConsoleForVideoGame/10
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ConsoleDto))]
        public IHttpActionResult ListConsoleForVideoGame(int id)
        {
            List<Consoles> Consoles = db.Consoles.Where(
                c => c.VideoGames.Any(
                    v => v.VideoGameID == id)
                    ).ToList();

            List<ConsoleDto> ConsoleDtos = new List<ConsoleDto>();

            Consoles.ForEach(c => ConsoleDtos.Add(new ConsoleDto()
            {
                ConsoleID = c.ConsoleID,
                ConsoleName = c.ConsoleName

            }));

            return Ok(ConsoleDtos);
        }

        // YOU DID NOT INCLUDED LIST CONSOLE NOT ASSIGN TO VIDEO GAME

        /// <summary>
        /// Returns Console in the system base on the ID
        /// </summary>
        /// <param name="id">Primary Key for Console</param>
        /// <returns>
        /// 200 Ok: Console content based on the ContentID in the system
        /// or
        /// 404 Error
        /// </returns>
        /// <example>
        // GET: api/ConsoleData/FindConsole/10
        /// </example>
        [ResponseType(typeof(Consoles))]
        [HttpGet]
        public IHttpActionResult FindConsole(int id)
        {
            Consoles Consoles = db.Consoles.Find(id);
            ConsoleDto ConsoleDto = new ConsoleDto()
            {
                ConsoleID = Consoles.ConsoleID,
                ConsoleName = Consoles.ConsoleName
            };
            if (Consoles == null)
            {
                return NotFound();
            }

            return Ok(ConsoleDto);
        }

        /// <summary>
        /// Update a Console content in the system with POST Data input
        /// </summary>
        /// <param name="id">Primary Key for the Console ID</param>
        /// <param name="Console">JSON FORM DATA of a Console</param>
        /// <returns>
        /// 204: Success no content response
        /// or
        /// 400: Bad request
        /// or
        /// 404 Not found
        /// </returns>
        /// <example>
        // PUT: api/ConsoleData/UpdateConsole/10
        // FORM DATA: Console JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateConsole(int id, Consoles Console)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Console.ConsoleID)
            {
                return BadRequest();
            }

            db.Entry(Console).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsolesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a Console to the syystem
        /// </summary>
        /// <param name="Console">JSON FORM DATA if a Console</param>
        /// <returns>
        /// 201: Creates Content ID and Console Data
        /// or 
        /// 400: Bad Request
        /// </returns>
        /// <example>
        // POST: api/ConsoleData/AddConsole
        // FORM DATA: Console JSON Object
        /// </example>
        [ResponseType(typeof(Consoles))]
        [HttpPost]
        public IHttpActionResult AddConsole(Consoles Console)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Consoles.Add(Console);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Console.ConsoleID }, Console);
        }

        /// <summary>
        /// Deletes a Console from the System
        /// </summary>
        /// <param name="id">Primary Key of a Console</param>
        /// <returns>
        /// 200 Ok
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        // DELETE: api/ConsoleData/DeleteConsole/10
        // FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Consoles))]
        [HttpPost]
        public IHttpActionResult DeleteConsole(int id)
        {
            Consoles Console = db.Consoles.Find(id);
            if (Console == null)
            {
                return NotFound();
            }

            db.Consoles.Remove(Console);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConsolesExists(int id)
        {
            return db.Consoles.Count(e => e.ConsoleID == id) > 0;
        }
    }
}