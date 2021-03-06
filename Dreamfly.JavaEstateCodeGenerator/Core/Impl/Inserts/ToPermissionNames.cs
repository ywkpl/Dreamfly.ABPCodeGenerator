﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl.Inserts
{
    public class ToPermissionNames : InsertToFile
    {
        public override void Insert()
        {
            int insertIndex = Code.LastIndexOf($"}}{NewLine}}}", StringComparison.Ordinal);
            if (insertIndex > 0)
            {
                string permissionCode = GetInsertCode();
                string insertedCode = Code.Insert(insertIndex, permissionCode);
                File.WriteAllText(FilePath, insertedCode);
            }
        }

        public override void Remove()
        {
            string permissionCode = GetInsertCode();
            string replacedCode = Code.Replace(permissionCode, "", StringComparison.InvariantCultureIgnoreCase);
            File.WriteAllText(FilePath, replacedCode);
        }

        public override string GetInsertCode()
        {
            return $@"{NewLine}{Tab}{Tab}//{entity.Description}
{Tab}{Tab}public const string Pages_{entity.Name} = ""Pages..{entity.Name}"";
{Tab}{Tab}public const string Pages_{entity.Name}_Create = ""Pages..{entity.Name}.Create"";
{Tab}{Tab}public const string Pages_{entity.Name}_Update = ""Pages..{entity.Name}.Update"";
{Tab}{Tab}public const string Pages_{entity.Name}_Delete = ""Pages..{entity.Name}.Delete"";{NewLine}{Tab}";
        }

        public ToPermissionNames(EntityDto entity) : base(entity)
        {
            FilePath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Authorization", "PermissionNames.cs");
            Code = ReadCode();
        }
    }
}
