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
    public class SessionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List All Sessions in the system
        /// </summary>
        /// <returns>
        /// 200 Ok: all sessions in the database and message associated with it
        /// </returns>
        /// <example>
        // GET: api/SessionData/ListSessions
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SessionDto))]
        public IHttpActionResult ListSessions()
        {
            List<Session> Sessions = db.Sessions.ToList();
            List<SessionDto> SessionDtos = new List<SessionDto>();

            Sessions.ForEach(s => SessionDtos.Add(new SessionDto()
            {
                SessionID = s.SessionID,
                SessionMsg = s.SessionMsg,
                VideoGameID  = s.VideoGameID,
                VideoGameName = s.VideoGames.VideoGameName

            }));

            return Ok(SessionDtos);
        }

        /// <summary>
        /// Find Session base one ID
        /// </summary>
        /// <param name="id">Primary Key of the Session</param>
        /// <returns>
        /// 200 Ok: A Session ID and Mesage assign to it
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        // GET: api/SessionData/FindSession/1
        /// </example>
        [ResponseType(typeof(Session))]
        [HttpGet]
        public IHttpActionResult FindSession(int id)
        {
            Session Session = db.Sessions.Find(id);
            SessionDto SessionDto = new SessionDto()
            {
                SessionID = Session.SessionID,
                SessionMsg = Session.SessionMsg,
                VideoGameID = Session.VideoGameID,
                VideoGameName = Session.VideoGames.VideoGameName
            };
            if (Session == null)
            {
                return NotFound();
            }

            return Ok(SessionDto);
        }

        /// <summary>
        /// List all sessions and content that matches to a video game id
        /// </summary>
        /// <returns>All session messages that relate to the video game id</returns>
        /// <param name="id">VideoGame ID</param>
        /// <example>
        //  GET: api/VideoGameData/ListSessionsForVideoGames/10  
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SessionDto))]
        public IHttpActionResult ListSessionsForVideoGames(int id)
        {
            List<Session> Session = db.Sessions.Where(v => v.VideoGameID == id).ToList();
            List<SessionDto> SessionDtos = new List<SessionDto>();

            Session.ForEach(v => SessionDtos.Add(new SessionDto()
            {
                SessionID = v.SessionID,
                SessionMsg = v.SessionMsg,
                VideoGameID = v.VideoGameID,
                VideoGameName = v.VideoGames.VideoGameName
            }));

            return Ok(SessionDtos);
        }

        /// <summary>
        /// Updates a particular session message in the system with POST Data input 
        /// </summary>
        /// <param name="id">Primary Key for Session</param>
        /// <param name="Session">JSON FORM DATA of a Session</param>
        /// <returns>
        /// 204: Success, no content response
        /// or
        /// 400: Bad Request
        /// or
        /// 404: Not Found
        /// </returns>
        /// <example>
        // POST: api/SessionData/UpdateSession/1
        // FORM DATA: Session JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSession(int id, Session Session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Session.SessionID)
            {
                return BadRequest();
            }

            db.Entry(Session).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                // An Error Occurs when updating the session message
                // It points to here.
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
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
        /// Adds a Session into the system
        /// </summary>
        /// <param name="Session">JSON FORM DATA of a Session</param>
        /// <returns>
        /// 201: Creates a Session ID and Message
        /// pr
        /// 400: Bad Request
        /// </returns>
        /// <example>
        // POST: api/SessionData/AddSession
        // FORM DATA: Session JSON Object
        /// </example>
        [ResponseType(typeof(Session))]
        [HttpPost]
        public IHttpActionResult AddSession(Session Session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sessions.Add(Session);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Session.SessionID }, Session);
        }

        /// <summary>
        /// Deletes a Session from the system base on ID
        /// </summary>
        /// <param name="id">Primary Key of the Session</param>
        /// <returns>
        /// 200 Ok
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        // DELETE: api/SessionData/DeleteSession/1
        // FORM DATA: empty
        /// </example>
        [ResponseType(typeof(Session))]
        [HttpPost]
        public IHttpActionResult DeleteSession(int id)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            db.Sessions.Remove(session);
            db.SaveChanges();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(int id)
        {
            return db.Sessions.Count(e => e.SessionID == id) > 0;
        }
    }
}