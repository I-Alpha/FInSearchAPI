using FInSearchAPI.Commands; 
using FluentValidation;

namespace FInSearchAPI.Validators
{
    public class OpenFigiIDValidator : AbstractValidator<GetSecurityLevelInfoCommand>
    {
        #region Constructors
        public OpenFigiIDValidator()
        { 
            RuleFor(x => x.Ticker)
                .NotEmpty().WithMessage("Command cannot be null.")
                .Length(2,7).WithMessage("Input must be company ticker. ");             
        }
        #endregion 
    }  
}
   
