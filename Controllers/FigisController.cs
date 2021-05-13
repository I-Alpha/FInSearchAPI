using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;

namespace FInSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FigisController : ControllerBase
    {
        private readonly FinSearchDBContext _context;

        public FigisController(FinSearchDBContext context)
        {
            _context = context;
        }

        // GET: api/Figis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Figi>>> GetFigi()
        {
            return await _context.Figi.ToListAsync();
        }

        // GET: api/Figis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Figi>> GetFigi(string id)
        {
            var figi = await _context.Figi.FindAsync(id);

            if (figi == null)
            {
                return NotFound();
            }

            return figi;
        }

        // PUT: api/Figis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFigi(string id, Figi figi)
        {
            if (id != figi.Figi_ID)
            {
                return BadRequest();
            }

            _context.Entry(figi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FigiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Figis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Figi>> PostFigi(Figi figi)
        {
            _context.Figi.Add(figi);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FigiExists(figi.Figi_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFigi", new { id = figi.Figi_ID }, figi);
        }

        // DELETE: api/Figis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFigi(string id)
        {
            var figi = await _context.Figi.FindAsync(id);
            if (figi == null)
            {
                return NotFound();
            }

            _context.Figi.Remove(figi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FigiExists(string id)
        {
            return _context.Figi.Any(e => e.Figi_ID == id);
        }
    }
}
