using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using Microsoft.Extensions.Logging;
using FInSearchAPI.Handlers;
using System.Threading;
using FInSearchAPI.Controllers.Commands;

namespace FInSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityLevelController : ControllerBase
    {
        #region fields
        private readonly FinSearchDBContext Context;
        private readonly GetSecurityLevelInfoCommandHandler GetSecurityLevelInfoCommandHandler;
        private readonly ILogger Logger;
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