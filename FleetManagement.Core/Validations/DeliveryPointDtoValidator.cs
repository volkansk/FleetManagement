using FleetManagement.Core.DTOs.Input;
using FluentValidation;

namespace FleetManagement.Core.Validations
{
    public class DeliveryPointDtoValidator : AbstractValidator<DeliveryPointDto>
    {
        public DeliveryPointDtoValidator()
        {
            RuleFor(x => x.type).IsInEnum().WithMessage("Must be in DeliveryPointTypes.");

            RuleFor(x => x.value).NotNull().WithMessage("{PropertyName} is required.")
                .InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0.");
        }
    }
}
