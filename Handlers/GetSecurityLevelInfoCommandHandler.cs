using FInSearchAPI.Commands;
using FInSearchAPI.Models.Requests;
using FInSearchAPI.Services;
using FinSearchDataAccessLibrary;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using FinSearchDataAPI;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FInSearchAPI.Handlers
{
    public class GetSecurityLevelInfoCommandHandler : IRequestHandler<GetSecurityLevelInfoCommand, IEnumerable<FigiInstrument>>
    {
        #region Fields
        private readonly FinSearchDBContext DbContext;
        private readonly IValidator<GetSecurityLevelInfoCommand> Validator;
        private readonly ILogger<GetSecurityLevelInfoCommandHandler> Logger;
        private readonly MappingService MappingService; 

        #endregion
        #region Contructors 
        public GetSecurityLevelInfoCommandHandler(FinSearchDBContext dbContext, IValidator<GetSecurityLevelInfoCommand> validator, ILogger<GetSecurityLevelInfoCommandHandler> logger, MappingService mappingService)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService)); 
        }

        #endregion
        #region Methods 
        public async Task<IEnumerable<FigiInstrument>> Handle(GetSecurityLevelInfoCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var results= new FigiInstrument();
            //validate id 
            var res = Validator.Validate(request);

            if (res.IsValid)
            {
                if (FigiExists(request.Ticker))
                    return new List<FigiInstrument> { DbContext.Figi.Where(x => x.Ticker == request.Ticker).FirstOrDefault() };

                string response = await MappingService.MapTickerToFigi(request.Ticker);
                if (response != null) //add response
                {
                    await AddFigiToDB(response);
                    results = DbContext.Figi.Where(x => x.Ticker == request.Ticker).FirstOrDefault();
                }
            }
            return new List<FigiInstrument>() { results };
        }

        private async Task AddFigiToDB(string response)
        {
            var figi = JsonConvert.DeserializeObject<List<FigiInstrument>>(response);
            await DbContext.Figi.AddAsync(figi[0]);
            await DbContext.SaveChangesAsync();
        }

        
        private bool FigiExists(string Ticker)
        {
            return DbContext.Figi.Any(e => e.Ticker ==  Ticker);
        }
        #endregion
    }
}