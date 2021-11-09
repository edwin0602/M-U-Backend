using FluentValidation;
using RestBackend.Core.Resources;

namespace RestBackend.Api.Validators
{
    public class SavePropertyResourceValidator : AbstractValidator<CreatePropertyResource>
    {
        public SavePropertyResourceValidator()
        {

            RuleFor(a => a.Name)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(a => a.CodeInternal)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(a => a.Price)
                .GreaterThan(0)
                .NotNull();

            RuleFor(a => a.Year)
                .GreaterThan(1000)
                .NotNull();

            RuleFor(a => a.Address)
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(a => a.IdOwner)
                .NotNull();
        }
    }
}
