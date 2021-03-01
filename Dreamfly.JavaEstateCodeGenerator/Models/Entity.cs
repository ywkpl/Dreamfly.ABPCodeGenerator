using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class Entity
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public bool HasIHasCompany { get; set; }
        public bool HasIHasTenant { get; set; }
        public Project Project { get; set; }
        public bool IsSync { get; set; }
        public List<EntityItem> EntityItems { get; set; }
    }

    public class EntityItem
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public decimal? Length { get; set; }
        public bool IsRequired { get; set; }
        public bool InQuery { get; set; }
        public bool InResponse { get; set; }
        public bool InCreate { get; set; }
        public string Description { get; set; }
    }
}