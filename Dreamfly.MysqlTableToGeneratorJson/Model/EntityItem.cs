using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamfly.MysqlTableToGeneratorJson
{
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
