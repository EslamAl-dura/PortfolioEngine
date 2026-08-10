using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Web.Models.VM
{
    public class EditTechCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public CategoryTypes Type { get; set; }
    }
}
