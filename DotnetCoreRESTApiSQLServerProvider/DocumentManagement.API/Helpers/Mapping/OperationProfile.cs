using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class OperationProfile : Profile
    {
        public OperationProfile()
        {
            CreateMap<Operation, OperationDto>().ReverseMap();
            CreateMap<AddOperationCommand, Operation>();
            CreateMap<UpdateOperationCommand, Operation>();
        }
    }
}
