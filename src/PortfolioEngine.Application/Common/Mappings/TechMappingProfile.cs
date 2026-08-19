using AutoMapper;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;

namespace PortfolioEngine.Application.Common.Mappings;

public class TechMappingProfile : Profile
{
    public TechMappingProfile()
    {
        // --- TechCategory Mappings ---
        CreateMap<TechCategory, TechCategoryDto>();
        CreateMap<CreateTechCategoryDto, TechCategory>();
        CreateMap<UpdateTechCategoryDto, TechCategory>(); // Added missing update mapping

        // --- Technology Mappings ---
        // Entity -> DTO
        CreateMap<Technology, TechnologyDto>()
            .ForMember(dest => dest.TechCategoryName, opt => opt.MapFrom(src => src.TechCategory!.Name));

        // DTOs -> Entity
        CreateMap<CreateTechnologyDto, Technology>();
        CreateMap<UpdateTechnologyDto, Technology>();

        // --- Contact Mappings ---
        CreateMap<Contact, ContactDto>();
            
        CreateMap<CreateContactDto, Contact>();
        CreateMap<UpdateContactDto, Contact>();

        // --- Skill Mappings ---
        CreateMap<Skill, SkillDto>();
        CreateMap<CreateSkillDto, Skill>()
            .ForMember(dest => dest.Technologies, opt => opt.Ignore());
        CreateMap<UpdateSkillDto, Skill>()
            .ForMember(dest => dest.Technologies, opt => opt.Ignore());

        // --- Colleague Mappings ---
        CreateMap<Colleague, ColleagueDto>();
        CreateMap<CreateColleagueDto, Colleague>();
        CreateMap<UpdateColleagueDto, Colleague>();

        // --- Project Mappings ---
        CreateMap<Project, ProjectDto>();
        CreateMap<Colleague, ColleagueLookupDto>();
        CreateMap<Skill, SkillLookupDto>();
        CreateMap<CreateProjectDto, Project>()
            .ForMember(dest => dest.Colleagues, opt => opt.Ignore())
            .ForMember(dest => dest.Skills, opt => opt.Ignore());
        CreateMap<UpdateProjectDto, Project>()
            .ForMember(dest => dest.Colleagues, opt => opt.Ignore())
            .ForMember(dest => dest.Skills, opt => opt.Ignore());

        // --- Messages Mapping
        CreateMap<Message, MessageDto>();
        // If I ever need DTO -> Entity mapping:
        // CreateMap<MessageDto, Message>();
    }
}