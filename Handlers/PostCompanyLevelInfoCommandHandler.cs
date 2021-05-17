using FInSearchAPI.Commands;
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
    public class PostCompanyLevelInfoCommandHandler //: IRequestHandler<PostCompanyLevelInfoCommand, IEnumerable<Company>>
    {
        #region Fields        
        private readonly FinSearchDBContext DbContext;
        private readonly IValidator<PostCompanyLevelInfoCommand> Validator;
        private readonly ILogger Logger;
        private readonly MappingService MappingService;
        #endregion 

        #region Constructors
        public PostCompanyLevelInfoCommandHandler(FinSearchDBContext _DbContext, ILogger<PostCompanyLevelInfoCommand> _Logger, IValidator<PostCompanyLevelInfoCommand> _Validator, MappingService _MappingService)
        {
            DbContext = _DbContext ?? throw new ArgumentNullException(nameof(_DbContext));
            Validator = _Validator ?? throw new ArgumentNullException(nameof(_Validator));
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger));
            MappingService = _MappingService ?? throw new ArgumentNullException(nameof(_MappingService));  
        }
        #endregion

        #region Methods 
        public async Task<IEnumerable<Company>> Handle(PostCompanyLevelInfoCommand request, CancellationToken cancellationToken)
        {

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            //validate id 
            var res = Validator.Validate(request);

            //perform logic 
            var company = await DbContext.Companies.FindAsync(res);
            if (company == null)
            {
                company = await MappingService.GetInfoForEntry(company);

            }
            return new List<Company>() { company };
        }




        #endregion
    }
} 