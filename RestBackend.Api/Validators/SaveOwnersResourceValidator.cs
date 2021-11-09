using FluentValidation;
using RestBackend.Core.Resources;

namespace RestBackend.Api.Validators
{
    public class SaveOwnersResourceValidator : AbstractValidator<CreateOwnerResource>
    {
        public SaveOwnersResourceValidator()
        {

            RuleFor(a => a.Name)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(a => a.Birthday)
                .NotNull();

            RuleFor(a => a.Address)
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
