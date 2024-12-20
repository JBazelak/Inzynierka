using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;

namespace Inzynierka.UI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contractor, ContractorDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Material, MaterialDto>().ReverseMap();
            CreateMap<UpdateContractorDto, Contractor>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
