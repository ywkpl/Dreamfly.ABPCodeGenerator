using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class RenderEntity
    {
        public string ProjectName { get; set; }
        public bool ProjectHasApi { get; set; }
        public bool ProjectIsShare { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        public bool EntityHasICompany { get; set; }
        public bool EntityHasITenant { get; set; }
        public string TableName { get; set; }
        public string ProjectPackagePath { get; set; }

        public List<EntityItemDto> EntityItems { get; set; }
        public Template Template { get; set; }
        public Author Author { get; set; }
    }
}