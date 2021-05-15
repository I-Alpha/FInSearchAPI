using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using FInSearchAPI.Handlers;
using System.Threading;
using Microsoft.Extensions.Logging;
using System; 
using FinSearchDataAccessLibrary;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using MediatR;

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
        private readonly IRequestHandler<GetCompanyLevelInfoCommand, IEnumerable<Company>> GetCompanyLevelInfoCommandHandler;
        private readonly IRequestHandler<PostCompanyLevelInfoCommand, IEnumerable<Company>> PostCompanyLevelInfoCommandHandler;
        #endregion

        #region Contructor

        public CompanyLevelController(ILogger<CompanyLevelController> _Logger, FinSearchDBContext _context, IRequestHandler<GetCompanyLevelInfoCommand, IEnumerable<Company>> _GetCompanyLevelInfoCommandHandler,  IRequestHandler<PostCompanyLevelInfoCommand, IEnumerable<Company>> _PostCompanyLevelInfoCommandHandler)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context)); ;
            GetCompanyLevelInfoCommandHandler = _GetCompanyLevelInfoCommandHandler ?? throw new ArgumentNullException(nameof(_GetCompanyLevelInfoCommandHandler)); ;
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger));
            PostCompanyLevelInfoCommandHandler = _PostCompanyLevelInfoCommandHandler ?? throw new ArgumentNullException(nameof(_GetCompanyLevelInfoCommandHandler)); ; ;
        }
        #endregion

        #region Methods 
        // GET: api/Companies
        [HttpGet]
        public ActionResult<IEnumerable<Company>> GetCompanies()
        {
            return  context.Companies.ToList();
        }

        // GET: api/Companies/5
        [HttpGet("{id:length(12)}", Name ="GetCompanyInfo")] 
        public ActionResult<List<Company>> GetCompanyInfoById([FromBody] GetCompanyLevelInfoCommand command, CancellationToken cancellationToken = default)
        {
            /*  if (command == null)
                  throw new ArgumentNullException(nameof(command));

              var res =  GetCompanyLevelInfoCommandHandler.Handle(command, cancellationToken).Result;

              if (res != null)
                  return Ok(res.Serialize());*/

            return new List<Company>();
        }


        /*// POST: api/Companies
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
