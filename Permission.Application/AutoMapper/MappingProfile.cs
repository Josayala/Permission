using AutoMapper;
using Permission.Application.CommandService.Commands;
using Permission.Infrastructure.Dtos;

namespace Permission.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {         
            this.CreateMap<PermissionDto,Domain.Models.Permission>();
            this.CreateMap<Domain.Models.Permission,PermissionDto>();
            this.CreateMap<CreatePermissionCommand, Domain.Models.Permission>()
                 .ForMember(d => d.PermissionDate, s => s.MapFrom(x => DateTime.Now));
        }


    }
}
