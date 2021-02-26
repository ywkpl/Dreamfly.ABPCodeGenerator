using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class TableSetting
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<TableFieldSetting> Fields { get; set; }
    }
}