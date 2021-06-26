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
using System.Diagnostics;
using System.Web;
using System.IO;

namespace PersonalVideoGameLibrary.Controllers
{
    public class VideoGameDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List All Video games in the system.
        /// </summary>
        /// <returns>All video games in the database.</returns>
        /// <example>
        // GET: api/VideoGameData/ListVideoGames
        ///</example>
        [HttpGet]
        [ResponseType(typeof(VideoGameDto))]
        public IHttpActionResult ListVideoGames()
        {
            List<VideoGames> VideoGames = db.VideoGames.ToList();
            List<VideoGameDto> VideoGameDtos = new List<VideoGameDto>();

            VideoGames.ForEach(v => VideoGameDtos.Add(new VideoGameDto()
            {
                VideoGameID = v.VideoGameID,
                VideoGameName = v.VideoGameName,
                VideoGamePrice = v.VideoGamePrice,
                VideoGameHours = v.VideoGameHours,
                VideoGameHasPic = v.VideoGameHasPic,
                PicExtension = v.PicExtension
            }));

            return Ok(VideoGameDtos);
        }

        /// <summary>
        /// List information about video games related to a particular console
        /// </summary>
        /// <param name="id">Console ID</param>
        /// <returns>All videos games and the consoles name that associate with it.</returns>
        /// <example>
        //  GET: api/VideoGameData/ListVideoGamesForConsoles/10 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(VideoGameDto))]
        public IHttpActionResult ListVideoGamesForConsoles(int id)
        {
            //All videogames that match with the consoles ID
            List<VideoGames> VideoGames = db.VideoGames.Where(
                v => v.Consoles.Any(
                        c => c.ConsoleID == id
                    )).ToList();
            List<VideoGameDto> VideoGameDtos = new List<VideoGameDto>();

            VideoGames.ForEach(v => VideoGameDtos.Add(new VideoGameDto()
            {
                VideoGameID = v.VideoGameID,
                VideoGameName = v.VideoGameName,
                VideoGamePrice = v.VideoGamePrice,
                VideoGameHours = v.VideoGameHours
            }));

            return Ok(VideoGameDtos);
        }

        /// <summary>
        /// Associates a particular console with a particular video game
        /// </summary>
        /// <param name="videogameid">The video game id primary key</param>
        /// <param name="consoleid">The console id primary key</param>
        /// <returns>
        /// If passed OK 202 if failed 404 error.
        /// </returns>
        /// <example>
        // POST api/VideoGameData/AssociateVideoGameWithConsole/10/4
        /// </example>
        [HttpPost]
        [Route("api/VideoGameData/AssignVideoGameWithConsole/{videogameid}/{consoleid}")]
        public IHttpActionResult AssignVideoGameWithConsole(int videogameid, int consoleid)
        {
            VideoGames SelectedVideoGame = db.VideoGames.Include(c => c.Consoles).Where(v => v.VideoGameID == videogameid).FirstOrDefault();
            Consoles SelectedConsoles = db.Consoles.Find(consoleid);

            if(SelectedVideoGame==null || SelectedConsoles == null)
            {
                return NotFound();
            }

            SelectedVideoGame.Consoles.Add(SelectedConsoles);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes a video game that is placed under a console
        /// </summary>
        /// <param name="videogameid">The VideoGame ID primary key</param>
        /// <param name="consoleid">The Console ID primary key</param>
        /// <returns>
        /// Either give 200 Message Ok or 404 Error Message.
        /// </returns>
        /// <example>
        // POST api/VideoGameData/UnAssociateVideoGameWithConsole/10/4
        /// </example>
        [HttpPost]
        [Route("api/VideoGameData/UnAssignVideoGameWithConsole/{videogameid}/{consoleid}")]
        public IHttpActionResult UnAssignVideoGameWithConsole(int videogameid, int consoleid)
        {
            VideoGames SelectedVideoGame = db.VideoGames.Include(c => c.Consoles).Where(v => v.VideoGameID == videogameid).FirstOrDefault();
            Consoles SelectedConsoles = db.Consoles.Find(consoleid);

            if (SelectedVideoGame == null || SelectedConsoles == null)
            {
                return NotFound();
            }

            SelectedVideoGame.Consoles.Add(SelectedConsoles);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Finds the video game base on the id entered
        /// </summary>
        /// <param name="id">Primary Key for the video game</param>
        /// <returns>
        /// Information base on the ID of the video game
        /// </returns>
        /// <example>
        // GET: api/VideoGameData/FindVideoGames/10
        /// </example>
        [ResponseType(typeof(VideoGames))]
        [HttpGet]
        public IHttpActionResult FindVideoGame(int id)
        {
            VideoGames VideoGame = db.VideoGames.Find(id);
            VideoGameDto VideoGameDto = new VideoGameDto()
            {
                VideoGameID = VideoGame.VideoGameID,
                VideoGameName = VideoGame.VideoGameName,
                VideoGamePrice = VideoGame.VideoGamePrice,
                VideoGameHours = VideoGame.VideoGameHours,
                VideoGameHasPic = VideoGame.VideoGameHasPic,
                PicExtension = VideoGame.PicExtension
            };
            if (VideoGame == null)
            {
                return NotFound();
            }

            return Ok(VideoGameDto);
        }

        /// <summary>
        /// Updates a video game base on the user input
        /// </summary>
        /// <param name="id">Primary Key of video game</param>
        /// <param name="videoGames">JSON FORM DATA of a video game</param>
        /// <returns>
        /// Success no content response: 204
        /// or
        /// Error message that could appear such as 400 or 404
        /// </returns>
        /// <example>
        // FORM DATA: VideoGame JSON Object
        // POST: api/VideoGameData/UpdateVideoGame/10
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateVideoGame(int id, VideoGames videoGames)
        {
            Debug.WriteLine("Update Video Game is connected");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != videoGames.VideoGameID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("Get parameter " + id);
                Debug.WriteLine("Post parameter "+ videoGames.VideoGameID);
                return BadRequest();
            }

            db.Entry(videoGames).State = EntityState.Modified;
            // Pictures update is handle by another method.
            db.Entry(videoGames).Property(v => v.VideoGameHasPic).IsModified = false;
            db.Entry(videoGames).Property(v => v.PicExtension).IsModified = false;

            // Problems where found here where the image does not get updated to the assign videogame.
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoGamesExists(id))
                {
                    Debug.WriteLine("VideoGame Not Found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions trigger");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Receives the videogame picture data, uploads it to the web page, and updates the videogame HasPic option
        /// </summary>
        /// <param name="id">Videogame id</param>
        /// <returns>Code 200 if successful</returns>
        /// <example>
        // curl -F videogamepic=@file.jpg "https://localhost:xx/api/VideoGameData/uploadvideogamepic/11"
        /// POST: api/VideoGameData/UploadVideoGamePic/11
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        [HttpPost]
        public IHttpActionResult UploadVideoGamePic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                // Test to see if the form data is received 
                Debug.WriteLine("Received multipart form data");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);
                // checks if the files have been received

                // Checks if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var videogamePic = HttpContext.Current.Request.Files[0];
                    // Check if the file is empty
                    if (videogamePic.ContentLength > 0)
                    {
                        // Establish valid file types and changed to other file extensions
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(videogamePic.FileName).Substring(1);
                        // Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                // File name is the id of the image
                                string fn = id + "." + extension;

                                // Get a direct file path to ~/Content/VideoGames/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/VideoGames/"), fn);

                                // Save file
                                videogamePic.SaveAs(path);

                                // If successfull then we will get the fields
                                haspic = true;
                                picextension = extension;

                                // Update the VideoGameHasPic and PicExtention fields in the database
                                VideoGames SelectedVideoGame = db.VideoGames.Find(id);
                                SelectedVideoGame.VideoGameHasPic = haspic;
                                SelectedVideoGame.PicExtension = extension;
                                db.Entry(SelectedVideoGame).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                // In the event that the image is not saved successfully 
                                // The following messages will appear in console
                                Debug.WriteLine("Videogame Image was not saved successfully");
                                Debug.WriteLine("Exception: " + ex);
                                return BadRequest();
                            }
                        }
                    }
                }

                return Ok();
            }
            else
            {
                // Not multipart form data
                return BadRequest();
            }
        }


        /// <summary>
        /// Adds a video game into the system
        /// </summary>
        /// <param name="videoGames">JSON FORM DATA of a video game</param>
        /// <returns>
        /// Creates a Video game with content and ID: 201
        /// or
        /// 400 bad request
        /// </returns>
        /// <example>
        // FORM DATA: Videogame JSON OBJECT
        // POST: api/VideoGameData/AddVideoGame
        /// </example>
        [ResponseType(typeof(VideoGames))]
        [HttpPost]
        public IHttpActionResult AddVideoGame(VideoGames videoGames)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VideoGames.Add(videoGames);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = videoGames.VideoGameID }, videoGames);
        }

        /// <summary>
        /// Deletes a videogame from the system base on ID
        /// </summary>
        /// <param name="id">Primary Key for Videogame</param>
        /// <returns>
        /// 200 Ok 
        /// or
        /// 404 Error
        /// </returns>
        /// <example>
        // FORM DATA: (empty)
        // POST: api/VideoGameData/DeleteVideoGame/10
        /// </example>
        [ResponseType(typeof(VideoGames))]
        [HttpPost]
        public IHttpActionResult DeleteVideoGame(int id)
        {
            VideoGames videoGames = db.VideoGames.Find(id);
            if (videoGames == null)
            {
                return NotFound();
            }

            // Added if statement to delete image from path if included
            if (videoGames.VideoGameHasPic && videoGames.PicExtension != "")
            {
                // Deletes image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/VideoGames/" + id + "." + videoGames.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    // Console Message to confirm file being deleted
                    Debug.WriteLine("File in process of being deleted");
                    System.IO.File.Delete(path);
                }
            }

            db.VideoGames.Remove(videoGames);
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

        private bool VideoGamesExists(int id)
        {
            return db.VideoGames.Count(e => e.VideoGameID == id) > 0;
        }
    }
}