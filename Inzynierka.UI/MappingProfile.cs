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
            CreateMap<Contractor, ContractorDto>().ReverseMap();
            CreateMap<UpdateContractorDto, Contractor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Material mappings
            CreateMap<Material, MaterialDto>().ReverseMap();
            CreateMap<CreateMaterialDto, Material>();
            CreateMap<UpdateMaterialDto, Material>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Project mappings
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials));
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.Contractor, opt => opt.Ignore()) // Ignorowanie mapowania Contractor
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
