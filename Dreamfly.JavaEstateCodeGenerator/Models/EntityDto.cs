using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class EntityDto
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public bool HasIHasCompany { get; set; }
        public bool HasIHasTenant { get; set; }
        public Project Project { get; set; }
        public bool IsSync { get; set; }
        public List<EntityItemDto> EntityItems { get; set; }
    }

    public class EntityItemDto
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public int? Length { get; set; }
        public string RelateType { get; set; }
        public string RelateEntityName { get; set; }
        public bool NeedForeignKey { get; set; }
        public bool IsRelateSelf { get; set; }
        public string ForeignKeyName { get; set; }
        public int? Fraction { get; set; }
        public bool HasTime { get; set; }
        public bool IsRequired { get; set; }
        public bool InQuery { get; set; }
        public bool InResponse { get; set; }
        public bool InCreate { get; set; }
        public string Description { get; set; }
    }
}