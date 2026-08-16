using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Web.Models.VM
{
    public class EditTechCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public CategoryTypes Type { get; set; }
    }
}
