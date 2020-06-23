using System;
using System.IO;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class FileInsertCode
    {
        public const string NEW_LINE = "\n";
        public const string TAB = "\t";
        private string _filePath;

        public void Insert(Entity entity)
        {
            _filePath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Authorization", "PermissionNames.cs");
            string code = ReadCode();
            int insertIndex = code.LastIndexOf($"}}{NEW_LINE}}}", StringComparison.Ordinal);
            if (insertIndex > 0)
            {
                string permissionCode = GetPermissionCode(entity);
                string insertedCode = code.Insert(insertIndex, permissionCode);
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
            string permissionCode = GetPermissionCode(entity);
            string replacedCode = code.Replace(permissionCode, "", StringComparison.InvariantCultureIgnoreCase);
            File.WriteAllText(_filePath, replacedCode);
        }

        private string GetPermissionCode(Entity entity)
        {
            return $@"{NEW_LINE}{TAB}{TAB}//{entity.Description}
{TAB}{TAB}public const string Pages_{entity.Module}_{entity.Name} = ""Pages.{entity.Module}.{entity.Name}"";
{TAB}{TAB}public const string Pages_{entity.Module}_{entity.Name}_Create = ""Pages.{entity.Module}.{entity.Name}.Create"";
{TAB}{TAB}public const string Pages_{entity.Module}_{entity.Name}_Update = ""Pages.{entity.Module}.{entity.Name}.Update"";
{TAB}{TAB}public const string Pages_{entity.Module}_{entity.Name}_Delete = ""Pages.{entity.Module}.{entity.Name}.Delete"";{NEW_LINE}{TAB}";
        }
    }
}