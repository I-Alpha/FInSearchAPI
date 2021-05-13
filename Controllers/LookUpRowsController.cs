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
    public class LookUpRowsController : ControllerBase
    {
        private readonly FinSearchDBContext _context;

        public LookUpRowsController(FinSearchDBContext context)
        {
            _context = context;
        }

        // GET: api/LookUpRows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LookUpRow>>> GetBloomBergLookUp()
        {
            return await _context.BloomBergLookUp.ToListAsync();
        }

        // GET: api/LookUpRows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LookUpRow>> GetLookUpRow(int id)
        {
            var lookUpRow = await _context.BloomBergLookUp.FindAsync(id);

            if (lookUpRow == null)
            {
                return NotFound();
            }

            return lookUpRow;
        }

        // PUT: api/LookUpRows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLookUpRow(int id, LookUpRow lookUpRow)
        {
            if (id != lookUpRow.E_id)
            {
                return BadRequest();
            }

            _context.Entry(lookUpRow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LookUpRowExists(id))
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

        // POST: api/LookUpRows
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LookUpRow>> PostLookUpRow(LookUpRow lookUpRow)
        {
            _context.BloomBergLookUp.Add(lookUpRow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLookUpRow", new { id = lookUpRow.E_id }, lookUpRow);
        }

        // DELETE: api/LookUpRows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLookUpRow(int id)
        {
            var lookUpRow = await _context.BloomBergLookUp.FindAsync(id);
            if (lookUpRow == null)
            {
                return NotFound();
            }

            _context.BloomBergLookUp.Remove(lookUpRow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LookUpRowExists(int id)
        {
            return _context.BloomBergLookUp.Any(e => e.E_id == id);
        }
    }
}
