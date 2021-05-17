using FInSearchAPI.Commands;
using FInSearchAPI.Interfaces;
using FInSearchAPI.Services;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FInSearchAPI.Handlers
{
    public class GetCompanyLevelInfoCommandHandler : IRequestHandler<GetCompanyLevelInfoCommand, IEnumerable<Company>>
    {
        #region Fields        
        private readonly FinSearchDBContext DbContext;
        private readonly IValidator<GetCompanyLevelInfoCommand> Validator;
        private readonly ILogger<GetCompanyLevelInfoCommandHandler> Logger;
        private readonly MappingService MappingService;
        #endregion

        #region Constructors
        public GetCompanyLevelInfoCommandHandler(FinSearchDBContext _DbContext, ILogger<GetCompanyLevelInfoCommandHandler>  _Logger, IValidator<GetCompanyLevelInfoCommand> _Validator, MappingService _MappingService)
        {
            DbContext = _DbContext ?? throw new ArgumentNullException(nameof(_DbContext));
            Validator = _Validator ?? throw new ArgumentNullException(nameof(_Validator));
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger));
            MappingService = _MappingService ?? throw new ArgumentNullException(nameof(_MappingService)); ;
        }
        #endregion

        #region Methods 
        public async Task<IEnumerable<Company>> Handle(GetCompanyLevelInfoCommand request, CancellationToken cancellationToken)
        {

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            //validate id 
            var res =   Validator.Validate(request);
            var company = new Company();
            var results = new List<Company>();

            if (res.IsValid)
            //perform logic 
            { 

                //checked if company exists
                if (CompanyExists(request.id))
                {
                    results = DbContext.Companies.Where(x=> x.PermId == request.id).ToList();
                    
                }
                if (results.Count == 0)
                {
                    company.PermId = request.id;
                    company = await MappingService.GetInfoForEntry(company).ConfigureAwait(false);

                   if (company != null)
                    {
                        results.Add(company);                      
                    }
                }
            }

            return results; 

        }
        #endregion 
            private bool CompanyExists(string id)
        {
            return DbContext.Companies.Any(e => e.PermId == id);
        }

    }
}
