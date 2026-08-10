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
    }
}