using FleetManagement.Core.DTOs.Input;
using FluentValidation;

namespace FleetManagement.Core.Validations
{
    public class PackageDtoValidator : AbstractValidator<PackageDto>
    {
        public PackageDtoValidator()
        {
            RuleFor(x => x.barcode).NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(11).WithMessage("Maximum length of {PropertyName} is 11");

            RuleFor(x => x.deliveryPointValue).NotNull().WithMessage("{PropertyName} is required.")
                .InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0.");

            RuleFor(x => x.volumetricWeight).NotNull().WithMessage("{PropertyName} is required.")
                .InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0.");
        }
    }
}
