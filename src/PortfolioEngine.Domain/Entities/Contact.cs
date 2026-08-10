using PortfolioEngine.Domain.Common;
using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Domain.Entities
{
    public class Contact: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public ContactTypes ContactType { get; set; } = ContactTypes.Other;
    }
}
