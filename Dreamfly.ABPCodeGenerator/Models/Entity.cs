using System.Collections.Generic;

namespace Dreamfly.ABPCodeGenerator.Models
{
    public class EntityInput
    {

    }

    public class Entity
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public List<EntityItem> EntityItems { get; set; }
    }

    public class EntityItem
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public int? Length { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
        public List<EntityItemMapType> MapTypes { get; set; }
    }

    public enum EntityItemMapType
    {
        CreateInput,
        Output,
        QueryInput
    }
}