using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FInSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class LookUpTableController : ControllerBase
    {
        #region fields
        private readonly FinSearchDBContext context;
        private readonly ILogger<LookUpTableController> Logger;
        private readonly IMediator _mediator;
        #endregion

        #region Contructor

        public LookUpTableController(ILogger<LookUpTableController> _Logger, FinSearchDBContext _context, IMediator mediator)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context)); ;
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger));
            _mediator = mediator;
        }
        #endregion

        #region Methods

        // GET: HomeController
        [HttpGet("BloomBergLookUpTable")]
        public ActionResult<IEnumerable<LookUpRow>> GetAllRows()
        {
            var result = context.BloomBergLookUp.ToList();
            if (result != null && result.Count != 0)
                return Ok(result);
            return Ok("No records found");
        }

        #endregion

    }
}