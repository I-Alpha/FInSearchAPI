using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using Microsoft.Extensions.Logging;
using FInSearchAPI.Handlers;
using System.Threading;
using FInSearchAPI.Commands;
using MediatR;

namespace FInSearchAPI.Controllers
{
    [Route("FinSearch/[controller]")]
    [ApiController]
    public class SecurityLevelController : ControllerBase
    {
        #region fields
        private readonly FinSearchDBContext Context;
        private readonly IRequestHandler<GetSecurityLevelInfoCommand, IEnumerable<Figi>> GetSecurityLevelInfoCommandHandler;
        private readonly ILogger Logger;

        public SecurityLevelController(ILogger _Logger, FinSearchDBContext _context, IRequestHandler<GetSecurityLevelInfoCommand, IEnumerable<Figi>> _GetSecurityLevelInfoCommandHandler)
        {
             
            Context = _context ?? throw new ArgumentNullException(nameof(_context)); ;
            GetSecurityLevelInfoCommandHandler = _GetSecurityLevelInfoCommandHandler ?? throw new ArgumentNullException(nameof(_GetSecurityLevelInfoCommandHandler)); ;
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger)); ;
        }
        #endregion



        #region Methods        
        // GET: api/Figi s
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Figi>>>GetFigis()  
        {
            return await Context.Figi.ToListAsync();
        }

        // GET: api/Figi s/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSecurityLevelInfo([FromBody] GetSecurityLevelInfoCommand command, CancellationToken cancellationToken = default)
        { 

            if (command  == null)
                throw new ArgumentNullException(nameof(command));

            var res = await GetSecurityLevelInfoCommandHandler.Handle(command, cancellationToken);

            if (res != null)
                return Ok(res);

            return Ok(1);
        }
        #endregion 
    }
}