using AutoMapper;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Entities;

namespace FleetManagement.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Bag
            CreateMap<BagDto, Bag>().ForMember(x => x.DeliveryPoint, opt => opt.MapFrom(model => model));
            CreateMap<BagDto, DeliveryPoint>().ForMember(x => x.Value, opt => opt.MapFrom(model => model.deliveryPointValue));
            CreateMap<Bag, BagResultDto>();
            #endregion

            #region DeliveryPoint
            CreateMap<DeliveryPointDto, DeliveryPoint>();
            CreateMap<DeliveryPoint, DeliveryPointResultDto>();
            #endregion

            #region Package
            CreateMap<PackageDto, Package>().ForMember(x => x.DeliveryPoint, opt => opt.MapFrom(model => model));
            CreateMap<PackageDto, DeliveryPoint>().ForMember(x => x.Value, opt => opt.MapFrom(model => model.deliveryPointValue));
            CreateMap<Package, PackageResultDto>();
            #endregion

            #region Vehicle
            CreateMap<VehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleResultDto>();
            #endregion

            #region Fleet - Distribution
            CreateMap<DistributionCommandDto, DistributionResultDto>();
            CreateMap<DistributionCommandDto.Route, DistributionResultDto.Route>();
            CreateMap<DistributionCommandDto.Route.Delivery, DistributionResultDto.Route.Delivery>();
            #endregion
        }
    }
}
