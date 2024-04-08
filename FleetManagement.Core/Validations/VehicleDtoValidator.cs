using FleetManagement.Core.DTOs.Input;
using FluentValidation;

namespace FleetManagement.Core.Validations
{
    public class VehicleDtoValidator : AbstractValidator<VehicleDto>
    {
        public VehicleDtoValidator()
        {
            RuleFor(x => x.plate).NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(11).WithMessage("Maximum length of {PropertyName} is 11");
        }
    }
}
