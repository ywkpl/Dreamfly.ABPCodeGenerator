using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class RenderEntity
    {
        public string ProjectName { get; set; }
        public string EntityName { get; set; }
        public string TableName { get; set; }
        public string ModuleName { get; set; }
        public string ProjectPackagePath { get; set; }

        public List<EntityItem> EntityItems { get; set; }
        public Template Template { get; set; }
        public Author Author { get; set; }
    }
}