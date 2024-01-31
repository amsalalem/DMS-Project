using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class ScreenOperationProfile : Profile
    {
        public ScreenOperationProfile()
        {
            CreateMap<ScreenOperation, ScreenOperationDto>().ReverseMap();
            CreateMap<AddScreenOperationCommand, ScreenOperation>().ReverseMap();
        }
    }
}
