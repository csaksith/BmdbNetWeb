using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BmdbNetWeb.Models;

namespace BmdbNetWeb.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CreditsController : ControllerBase {
        private readonly BmdbContext _context;

        public CreditsController(BmdbContext context) {
            _context=context;
        }

        // GET: api/Credits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Credit>>> GetCredits() {
            var credits = _context.Credits.Include(c => c.Actor)
                                          .Include(c => c.Movie);
                return await credits.ToListAsync();
        }
 
        // GET: api/Credits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Credit>> GetCredit(int id) {
            var credit = await _context.Credits.Include(c => c.Actor)
                                               .Include(c => c.Movie)
                                               .FirstOrDefaultAsync(c => c.Id==id);
            if (credit==null) {
                return NotFound();
            }

            return credit;
        }
      

        // PUT: api/Credits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCredit(int id,Credit credit) {
            if (id!=credit.Id) {
                return BadRequest();
            }

            _context.Entry(credit).State=EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!CreditExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Credits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Credit>> PostCredit(Credit credit) {
            _context.Credits.Add(credit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCredit",new { id = credit.Id },credit);
        }

        // DELETE: api/Credits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredit(int id) {
            var credit = await _context.Credits.FindAsync(id);
            if (credit==null) {
                return NotFound();
            }

            _context.Credits.Remove(credit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Credits
        [HttpGet("movie-id/{movieId}")]
        public async Task<ActionResult<IEnumerable<Credit>>> GetCreditsForMovie(int movieId) {
            // SELECT * FROM Credit WHERE MovieId = movieId
            var credits = _context.Credits.Include(c => c.Actor)
                                          .Include(c => c.Movie)
                                          .Where(c => c.MovieId==movieId);
            return await credits.ToListAsync();
        }

        private bool CreditExists(int id) {
            return _context.Credits.Any(e => e.Id==id);
        }
    }
}
