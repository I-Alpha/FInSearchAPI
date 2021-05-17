using FInSearchAPI.Commands;
using FInSearchAPI.Models.Requests;
using FInSearchAPI.Services;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FInSearchAPI.Handlers
{
    public class GetSecurityLevelInfoCommandHandler : IRequestHandler<GetSecurityLevelInfoCommand, IEnumerable<FigiInstrument>>
    {
        private readonly FinSearchDBContext DbContext;
        private readonly IValidator<GetSecurityLevelInfoCommand> Validator;
        private readonly ILogger<GetSecurityLevelInfoCommandHandler> Logger;
        private readonly MappingService MappingService;
        private readonly OpenFigiService OpenFigiService;

        public GetSecurityLevelInfoCommandHandler(FinSearchDBContext dbContext, IValidator<GetSecurityLevelInfoCommand> validator, ILogger<GetSecurityLevelInfoCommandHandler> logger, MappingService mappingService, OpenFigiService openFigiService)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
            OpenFigiService = openFigiService ?? throw new ArgumentNullException(nameof(openFigiService));
        }


        public async Task<IEnumerable<FigiInstrument>> Handle(GetSecurityLevelInfoCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            //validate id 
            var res = Validator.Validate(request);
            //perform logic 

            var figi = await DbContext.Figi.FindAsync(res);
            if (figi == null)
            {
                var temp = new List<FigiRequest> {
                        new FigiRequest() {IdValue = request.id
                    }};

                var response = await OpenFigiService.MapAsync(temp);
                if (response != null)
                    figi = response[0];
            }
            return new List<FigiInstrument>() { figi };
        }

    }
}