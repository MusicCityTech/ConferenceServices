using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using ConferenceWebAPI.Models;
using ConferenceWebAPI.DAL;

namespace ConferenceWebAPI.Controllers
{
    public class SpeakersController : ODataController
    {
        private readonly ConferenceContext _db = new ConferenceContext();

        // GET: odata/Speakers
        [EnableQuery]
        public IQueryable<Speaker> GetSpeakers()
        {
            return _db.Speakers;
        }

        // GET: odata/Speakers(5)
        [EnableQuery]
        public SingleResult<Speaker> GetSpeaker([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Speakers.Where(speaker => speaker.ID == key));
        }

        // PUT: odata/Speakers(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Speaker> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Speaker speaker = await _db.Speakers.FindAsync(key);
            if (speaker == null)
            {
                return NotFound();
            }

            patch.Put(speaker);

            try
            {
                await _db.SaveChangesAsync();
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

            _db.Speakers.Add(speaker);
            await _db.SaveChangesAsync();

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

            Speaker speaker = await _db.Speakers.FindAsync(key);
            if (speaker == null)
            {
                return NotFound();
            }

            patch.Patch(speaker);

            try
            {
                await _db.SaveChangesAsync();
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
            Speaker speaker = await _db.Speakers.FindAsync(key);
            if (speaker == null)
            {
                return NotFound();
            }

            _db.Speakers.Remove(speaker);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpeakerExists(int key)
        {
            return _db.Speakers.Count(e => e.ID == key) > 0;
        }
    }
}
