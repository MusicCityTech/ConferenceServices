using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using ConferenceWebAPI.Models;
using ConferenceWebAPI.DAL;

namespace ConferenceWebAPI.Controllers
{
    public class SpeakersController : ODataController
    {
        private ConferenceContext db = new ConferenceContext();

        // GET: odata/Speakers
        [EnableQuery]
        public IQueryable<Speaker> GetSpeakers()
        {
            return db.Speakers;
        }

        // GET: odata/Speakers(5)
        [EnableQuery]
        public SingleResult<Speaker> GetSpeaker([FromODataUri] int key)
        {
            return SingleResult.Create(db.Speakers.Where(speaker => speaker.ID == key));
        }

        // PUT: odata/Speakers(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Speaker> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Speaker speaker = await db.Speakers.FindAsync(key);
            if (speaker == null)
            {
                return NotFound();
            }

            patch.Put(speaker);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeakerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(speaker);
        }

        // POST: odata/Speakers
        public async Task<IHttpActionResult> Post(Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Speakers.Add(speaker);
            await db.SaveChangesAsync();

            return Created(speaker);
        }

        // PATCH: odata/Speakers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Speaker> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Speaker speaker = await db.Speakers.FindAsync(key);
            if (speaker == null)
            {
                return NotFound();
            }

            patch.Patch(speaker);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeakerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(speaker);
        }

        // DELETE: odata/Speakers(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Speaker speaker = await db.Speakers.FindAsync(key);
            if (speaker == null)
            {
                return NotFound();
            }

            db.Speakers.Remove(speaker);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpeakerExists(int key)
        {
            return db.Speakers.Count(e => e.ID == key) > 0;
        }
    }
}
