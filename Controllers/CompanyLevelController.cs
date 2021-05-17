using System.Collections.Generic;
using System.Linq; 
using Microsoft.AspNetCore.Mvc; 
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess; 
using System.Threading;
using Microsoft.Extensions.Logging;
using System;  
using System.Net.Mime; 
using MediatR;
using System.Threading.Tasks;

namespace FInSearchAPI.Commands
{
    [Route("FinSearch/[Controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class CompanyLevelController : ControllerBase
    {
        #region fields
        private readonly FinSearchDBContext context;
        private readonly ILogger<CompanyLevelController> Logger;
        private readonly IMediator _mediator;
        #endregion

        #region Contructor

        public CompanyLevelController(ILogger<CompanyLevelController> _Logger, FinSearchDBContext _context, IMediator mediator)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context)); ;
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger));
            _mediator = mediator;
        }
        #endregion

        #region Methods 
        // GET: api/CompanyLevel/All
        [HttpGet("All")]
        public ActionResult<IEnumerable<Company>> GetCompanies()
        {
            var r = context.Companies.ToList();
            if (r.Count() == 0)
                return Ok("No records exist");
            return Ok(r) ?? Ok("null error");
        }

        // GET: api/CompanyLevel/5000013918
        [HttpGet]
        [Route("id={id}")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanyLevelInfoById([FromRoute]GetCompanyLevelInfoCommand command, CancellationToken cancellationToken = default)
        { 
            /*  if (command == null)
                  throw new ArgumentNullException(nameof(command));
            */
              var res =await _mediator.Send(command).ConfigureAwait(false); 

              if (res != null)
                  return Ok(res); 

            return Ok("No such records exist");
        }


        /*// POST: api/CompanyLevel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCompanyLevelInfo([FromBody] PostCompanyLevelInfoCommand command, CancellationToken cancellationToken = default)
        {


            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var res = await PostCompanyLevelInfoCommandHandler.Handle(command, cancellationToken);

            if (res != null)
                return Ok(res);

            return Ok(1); 
             
        } */

        #endregion
    }
}
