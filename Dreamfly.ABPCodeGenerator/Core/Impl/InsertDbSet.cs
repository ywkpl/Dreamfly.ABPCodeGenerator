using Dreamfly.ABPCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class InsertDbSet
    {
        public const string NEW_LINE = "\n";
        public const string TAB = "\t";
        private string _filePath;
        private const string ENTITY_MARKUP_STRING = "/* Define a DbSet for each entity of the application */";

        public void Insert(Entity entity)
        {
            _filePath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.EntityFrameworkCore", "EntityFrameworkCore", "ReEstateDbContext.cs");
            string code = ReadCode();
            int insertIndex = code.IndexOf(ENTITY_MARKUP_STRING, StringComparison.Ordinal);
            if (insertIndex > 0)
            {
                string insertCode = GetInsertCode(entity);
                string insertedCode = code.Insert(insertIndex+ ENTITY_MARKUP_STRING.Length, insertCode);
                File.WriteAllText(_filePath, insertedCode);
            }
        }

        private string ReadCode()
        {
            return File.ReadAllText(_filePath);
        }

        public void Remove(Entity entity)
        {
            _filePath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Authorization", "PermissionNames.cs");
            string code = ReadCode();
            string insertCode = GetInsertCode(entity);
            string replacedCode = code.Replace(insertCode, "", StringComparison.InvariantCultureIgnoreCase);
            File.WriteAllText(_filePath, replacedCode);
        }

        private string GetInsertCode(Entity entity)
        {
            return $@"{NEW_LINE}{TAB}{TAB}public virtual DbSet<{entity.Name}> {entity.Name} {{ get; set; }}";
        }
    }
}
