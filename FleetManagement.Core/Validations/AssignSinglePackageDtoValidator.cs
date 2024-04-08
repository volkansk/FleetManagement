using FleetManagement.Core.DTOs.Input;
using FluentValidation;

namespace FleetManagement.Core.Validations
{
    public class AssignSinglePackageDtoValidator : AbstractValidator<AssignSinglePackageDto>
    {
        public AssignSinglePackageDtoValidator()
        {
            RuleFor(x => x.PackageBarcode).NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(11).WithMessage("Maximum length of {PropertyName} is 11");

            RuleFor(x => x.BagBarcode).NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(11).WithMessage("Maximum length of {PropertyName} is 11");
        }
    }
}
