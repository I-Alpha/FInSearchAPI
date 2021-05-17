using FInSearchAPI.Commands; 
using FluentValidation;

namespace FInSearchAPI.Validators
{
    public class OpenFigiIDValidator : AbstractValidator<GetSecurityLevelInfoCommand>
    {
        #region Constructors
        public OpenFigiIDValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Command cannot be null.")
                .Length(10).WithMessage("Input must be 10 digit permID. ");             
        }
        #endregion 
    }  
}
   
