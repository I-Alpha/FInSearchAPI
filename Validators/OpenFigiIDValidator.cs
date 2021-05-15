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

            Transform<string, string>(from: x => x.id, to: value => TransformId(value));

            RuleFor(x => x.id)
                .NotEmpty().WithMessage("invalid id, empty").NotNull().WithMessage("invalid id, null returned").Length(12).WithMessage("ID not length : 12 after transform");
        }
        #endregion
        private string TransformId(string ID)
        {
            var value = int.TryParse(ID, out int val) ? (int?)val : null;

            if (value == null)
                return new string("");

            return "1-" + ID;
        }

    }  
}
   
