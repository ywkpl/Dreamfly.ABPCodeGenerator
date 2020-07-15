using System.Collections.Generic;

namespace Dreamfly.ABPCodeGenerator.Models
{
    public class RenderEntity
    {
        public string ProjectName { get; set; }
        public string EntityName { get; set; }
        public string TableName { get; set; }
        public string ModuleName { get; set; }
        
        public List<EntityItem> EntityItems { get; set; }
        public Template Template { get; set; }
        public Author Author { get; set; }
    }
}