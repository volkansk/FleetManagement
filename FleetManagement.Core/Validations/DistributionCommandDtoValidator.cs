using FleetManagement.Core.DTOs.Input;
using FluentValidation;

namespace FleetManagement.Core.Validations
{
    public class DistributionCommandDtoValidator : AbstractValidator<DistributionCommandDto>
    {
        public DistributionCommandDtoValidator()
        {
            RuleFor(x => x.plate).NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(11).WithMessage("Maximum length of {PropertyName} is 11.");

            RuleForEach(x => x.route).SetValidator(new RouteValidator());
        }
    }

    public class RouteValidator : AbstractValidator<DistributionCommandDto.Route>
    {
        public RouteValidator()
        {
            RuleFor(x => x.deliveryPoint).NotNull().WithMessage("{PropertyName} is required.")
                .InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0.");
            RuleForEach(x => x.deliveries).SetValidator(new DeliveryValidator());
        }
    }

    public class DeliveryValidator : AbstractValidator<DistributionCommandDto.Route.Delivery>
    {
        public DeliveryValidator()
        {
            RuleFor(x => x.barcode).NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(11).WithMessage("Maximum length of {PropertyName} is 11.({PropertyValue})");
        }
    }
}
