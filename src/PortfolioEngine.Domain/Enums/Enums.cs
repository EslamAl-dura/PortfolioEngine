using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Domain.Enums;

public enum CategoryTypes
{
    [Display(Name = "Frontend")]
    Frontend = 1,
    [Display(Name = "Backend")]
    Backend = 2,
    [Display(Name = "Database")]
    Database = 3,
    [Display(Name = "DevOps")]
    DevOps = 4,
    [Display(Name = "OS")]
    OS = 5,
    [Display(Name = "Other")]
    Other = 0
}

public enum ContactTypes
{
    [Display(Name = "Email")]
    Email = 1,
    [Display(Name = "Phone")]
    Phone = 2,
    [Display(Name = "Social Media")]
    SocialMedia = 3,
    [Display(Name = "Exhibition")]
    Exhibition = 4,
    [Display(Name = "WebSite")]
    WebSite = 5,
    [Display(Name = "Other")]
    Other = 0
}