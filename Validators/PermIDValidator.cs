using FInSearchAPI.Commands;
using FInSearchAPI.Handlers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FInSearchAPI.Validators
{
    public class PermIDValidator : AbstractValidator<GetCompanyLevelInfoCommand>
    {
        #region Constructors
        public PermIDValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Command cannot be null.")
                .Length(10).WithMessage("Input must be 10 digit permID. ");
 }
        #endregion
        
        }   
     
}
   
