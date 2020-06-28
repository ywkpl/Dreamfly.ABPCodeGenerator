using Dreamfly.ABPCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Core.Interface;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class ToDbContext : InsertToFile
    {
        private const string ENTITY_MARKUP_STRING = "/* Define a DbSet for each entity of the application */";
        private const string ENTITY_NAMESPACE_STRING = "\nnamespace ";

        private void InsertDbSetToCode()
        {
            int insertIndex = Code.IndexOf(ENTITY_MARKUP_STRING, StringComparison.Ordinal);
            if (insertIndex > 0)
            {
                string insertCode = GetInsertCode();
                Code = Code.Insert(insertIndex + ENTITY_MARKUP_STRING.Length, insertCode);
            }
        }

        private void InsertUsingToCode()
        {
            string usingCode = GetUsingCode();
            int index = Code.IndexOf(usingCode, StringComparison.Ordinal);
            if (index < 0)
            {
                int insertIndex = Code.IndexOf(ENTITY_NAMESPACE_STRING, StringComparison.Ordinal);
                if (insertIndex >= 0)
                {
                    Code = Code.Insert(insertIndex, usingCode);
                }
            }
        }

        public override void Insert()
        {
            InsertDbSetToCode();
            //using插入
            InsertUsingToCode();
            WriteToFile(Code);
        }

        public override void Remove()
        {
            string insertCode = GetInsertCode();
            Code = Code.Replace(insertCode, "", StringComparison.InvariantCultureIgnoreCase);

            string usingCode = GetUsingCode();
            Code = Code.Replace(usingCode, "", StringComparison.InvariantCultureIgnoreCase);
            WriteToFile(Code);
        }

        private string GetUsingCode()
        {
            return $"using {entity.Project.Name}.{entity.Module};\n";
        }

        public override string GetInsertCode()
        {
            return $@"{NewLine}{Tab}{Tab}public virtual DbSet<{entity.Name}> {entity.Name} {{ get; set; }}";
        }

        public ToDbContext(Entity entity) : base(entity)
        {
            FilePath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.EntityFrameworkCore", "EntityFrameworkCore", "ReEstateDbContext.cs");
            Code = ReadCode();
        }
    }
}