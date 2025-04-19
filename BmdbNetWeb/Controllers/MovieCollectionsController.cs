using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BmdbNetWeb.Models;

namespace BmdbNetWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieCollectionsController : ControllerBase {
        private readonly BmdbContext _context;

        public MovieCollectionsController(BmdbContext context) {
            _context=context;
        }

        // GET: api/MovieCollections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieCollection>>> GetMovieCollections() {
            return await _context.MovieCollections.ToListAsync();
        }

        // GET: api/MovieCollections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieCollection>> GetMovieCollection(int id) {
            var movieCollection = await _context.MovieCollections.FindAsync(id);

            if (movieCollection==null) {
                return NotFound();
            }

            return movieCollection;
        }

        // PUT: api/MovieCollections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieCollection(int id,MovieCollection movieCollection) {
            if (id!=movieCollection.Id) {
                return BadRequest();
            }

            _context.Entry(movieCollection).State=EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
                // call RecalculateCollectionValue()

            }
            catch (DbUpdateConcurrencyException) {
                if (!MovieCollectionExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MovieCollections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieCollection>> PostMovieCollection(MovieCollection movieCollection) {
            _context.MovieCollections.Add(movieCollection);
            await _context.SaveChangesAsync();
            RecalculateCollectionValue(movieCollection.UserId);

            return CreatedAtAction("GetMovieCollection",new { id = movieCollection.Id },movieCollection);
        }

        // DELETE: api/MovieCollections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieCollection(int id) {
            var movieCollection = await _context.MovieCollections.FindAsync(id);
            if (movieCollection==null) {
                return NotFound();
            }

            _context.MovieCollections.Remove(movieCollection);
            await _context.SaveChangesAsync();
            // call RecalculateCollectionValue()

            return NoContent();
        }

        private bool MovieCollectionExists(int id) {
            return _context.MovieCollections.Any(e => e.Id==id);
        }

        private void RecalculateCollectionValue(int userId) {
            // insert, update, delete has already occurred
            var user = _context.Users.Find(userId);
            // get all MC records for this user
            var mcrs = _context.MovieCollections.Where(mc => mc.UserId==userId);
            // loop through all MCs and sum all the purchase price.
            decimal sum = 0;
            foreach (MovieCollection mc in mcrs) {
                sum+=mc.PurchasePrice;
            }
            // set the sum in the User.CollectionValue property
            user.CollectionValue=sum;
            // save the user record
            _context.SaveChanges();
        }
    }
}
