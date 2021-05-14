using FInSearchAPI.Handlers;
using FInSearchAPI.Interfaces;
using FInSearchAPI.Services;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FInSearchAPI.Handlers
{
    public class GetCompanyLevelInfoCommandHandler : IRequestHandler<GetCompanyLevelInfoCommand, Company>
    {
        #region Fields        
        private readonly FinSearchDBContext DbContext;
        private readonly IValidator<GetCompanyLevelInfoCommand> Validator;
        private readonly ILogger Logger;
        private readonly MappingService MappingService;
        #endregion

        #region Constructors
        public GetCompanyLevelInfoCommandHandler(FinSearchDBContext _DbContext,ILogger<GetCompanyLevelInfoCommand> _Logger,IValidator<GetCompanyLevelInfoCommand>_Validator, MappingService _MappingService)
        {
            DbContext = _DbContext ?? throw new ArgumentNullException(nameof(_DbContext)); 
            Validator = _Validator ?? throw new ArgumentNullException(nameof(_Validator));
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger));
            MappingService = _MappingService;
        }
        #endregion
   
        #region Methods 
        public async Task<Company> Handle(GetCompanyLevelInfoCommand request, CancellationToken cancellationToken )
        {

            if (request == null) 
               throw new ArgumentNullException(nameof(request));

            //validate id 
            var res = Validator.Validate(request);

           //perform logic 
            var company =   await DbContext.Companies.FindAsync(res);
            if (company == null)
            {
                company = await MappingService.GetInfoForEntry(company);              
                
            }
            return company;
        }
         
        #endregion
    }
}
