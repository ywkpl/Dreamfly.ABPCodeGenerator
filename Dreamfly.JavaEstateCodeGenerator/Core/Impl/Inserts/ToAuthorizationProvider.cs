using System;
using System.IO;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl.Inserts
{
    public class ToAuthorizationProvider : InsertToFile
    {
        public override void Insert()
        {
            int insertIndex = Code.LastIndexOf($"}}{NewLine}    }}{NewLine}}}", StringComparison.Ordinal);
            if (insertIndex > 0)
            {
                string permissionCode = GetInsertCode();
                string insertedCode = Code.Insert(insertIndex, permissionCode);
                WriteToFile(insertedCode);
            }
        }

        public override void Remove()
        {
            string permissionCode = GetInsertCode();
            string replacedCode = Code.Replace(permissionCode, "", StringComparison.InvariantCultureIgnoreCase);
            WriteToFile(replacedCode);
        }

        public override string GetInsertCode()
        {
            return $@"{NewLine}{Tab}{Tab}{Tab}//{entity.Description}
{Tab}{Tab}{Tab}var page{entity.Module}{entity.Name}=context.CreatePermission(PermissionNames.Pages_{entity.Module}_{entity.Name}, L(""Pages_{entity.Module}_{entity.Name}""));
{Tab}{Tab}{Tab}page{entity.Module}{entity.Name}.CreateChildPermission(PermissionNames.Pages_{entity.Module}_{entity.Name}_Create, L(""Add""));
{Tab}{Tab}{Tab}page{entity.Module}{entity.Name}.CreateChildPermission(PermissionNames.Pages_{entity.Module}_{entity.Name}_Update, L(""Edit""));
{Tab}{Tab}{Tab}page{entity.Module}{entity.Name}.CreateChildPermission(PermissionNames.Pages_{entity.Module}_{entity.Name}_Delete, L(""Delete""));{NewLine}{Tab}{Tab}";
        }

        public ToAuthorizationProvider(Entity entity) : base(entity)
        {
            FilePath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Authorization", "ReEstateAuthorizationProvider.cs");

            Code = ReadCode();
        }
    }
}