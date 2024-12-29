using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;

namespace Inzynierka.UI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Contractor mappings
            CreateMap<Contractor, ContractorDto>()
                .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects));
            CreateMap<ContractorDto, Contractor>();
            CreateMap<RegisterContractorDto, Contractor>().ReverseMap();

            // Material mappings
            CreateMap<Material, MaterialDto>().ReverseMap();
            CreateMap<CreateMaterialDto, Material>();
            CreateMap<UpdateMaterialDto, Material>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Project mappings
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.Contractor, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
