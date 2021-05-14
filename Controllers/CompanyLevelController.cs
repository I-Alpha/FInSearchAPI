using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using FInSearchAPI.Handlers;
using MediatR;
using System.Threading;
using Microsoft.Extensions.Logging;
using System;

namespace FInSearchAPI.Commands
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyLevelController : ControllerBase
    {
        #region fields
        private readonly FinSearchDBContext context;
        private readonly ILogger<CompanyLevelController> Logger;
        private readonly GetCompanyLevelInfoCommandHandler GetCompanyLevelInfoCommandHandler;
        #endregion

        #region Contructor

        public CompanyLevelController(ILogger<CompanyLevelController> _Logger, FinSearchDBContext _context, GetCompanyLevelInfoCommandHandler _GetCompanyLevelInfoCommandHandler)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context)); ;
            GetCompanyLevelInfoCommandHandler = _GetCompanyLevelInfoCommandHandler ?? throw new ArgumentNullException(nameof(_GetCompanyLevelInfoCommandHandler)); ;
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger)); ;
        }
        #endregion

        #region Methods 
        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyLevelInfo([FromBody] GetCompanyLevelInfoCommand command, CancellationToken cancellationToken = default)
        {


            if (command == null)
                throw new ArgumentNullException(nameof(command)); 
            
            var res = await GetCompanyLevelInfoCommandHandler.Handle(command, cancellationToken);
          
            if (res != null)
                return Ok(res);
      
            return Ok(1); 
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(string id, Company company)
        {
            if (id != company.OrganizationName)
            {
                return BadRequest();
            }

            context.Entry(company).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            context.Companies.Add(company);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyExists(company.OrganizationName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCompany", new { id = company.OrganizationName }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var company = await context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            context.Companies.Remove(company);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(string id)
        {
            return context.Companies.Any(e => e.OrganizationName == id);
        }

        #endregion
    }
}
