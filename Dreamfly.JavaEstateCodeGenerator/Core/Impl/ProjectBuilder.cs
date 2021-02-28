using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Core.Impl.Inserts;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl
{
    public class ProjectBuilder : IProjectBuilder
    {

        /* project.json 中 Project下OutputPath節點 配置：
         * Windows版本："E:\\Development\\Newcity\\microestate\\baseinfo-service\\src",
         * Docker linux版本：項目PropertyGroup需要設定
         * <DockerfileRunArguments>-v /f/Development/Newcity/microestate:/share</DockerfileRunArguments>
         * 即設定磁盤映射
         * 設定值："/share/baseinfo-service/src"
         */
        private readonly ITemplateEngine _templateEngine;
        private readonly ILogger<ProjectBuilder> _logger;

        public ProjectBuilder(ITemplateEngine templateEngine, ILogger<ProjectBuilder> logger)
        {
            _templateEngine = templateEngine;
            _logger = logger;

            //注册StringLowercase助手
            Handlebars.RegisterHelper("StringLowercase",
                (writer, context, parameters) => { writer.Write(parameters[0].ToString()?.ToLower()); });
        }

        public async Task Build(Entity entity)
        {
            await TryBuild(entity);
        }

        public Task Remove(Entity entity)
        {
            return Task.Run(() =>
            {
                //刪除產生代碼文件
                List<Template> templates = entity.Project.Templates.ToList();
                foreach (var template in templates)
                {
                    RemoveCodeFile(entity, template);
                }

                //刪除插入代碼
                //var insertCodes = GetInsertCodeBases(entity);
                //insertCodes.ForEach(p => p.Remove());
            });
        }

        private void RemoveCodeFile(Entity entity, Template template)
        {
            var fileName = Handlebars.Compile(template.OutputName)(entity);
            string outputPath = MakeCodeFilePath(entity, template);
            FileHelper.DeleteFile(outputPath, fileName);
        }

        private void InsertCodeToFile(Entity entity)
        {
            var insertCodes = GetInsertCodeBases(entity);
            insertCodes.ForEach(p => p.Remove());
            insertCodes.ForEach(p => p.Insert());
        }

        private static List<InsertCodeToFileBase> GetInsertCodeBases(Entity entity)
        {
            List<InsertCodeToFileBase> insertCodes = new List<InsertCodeToFileBase>
            {
//                new ToPermissionNames(entity),
//                new ToDbContext(entity),
//                new ToAuthorizationProvider(entity)
            };
            return insertCodes;
        }

        private async Task TryBuild(Entity entity)
        {
            try
            {
                List<Template> templates = entity.Project.Templates.Where(t => t.IsExecute).ToList();
                foreach (var template in templates)
                {
                    await GenerateCodeFile(entity, template);
                }

                //InsertCodeToFile(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
                throw;
            }
        }

        private string MakeCodeFilePath(Entity entity, Template template)
        {
            var folder = Handlebars.Compile(template.OutputFolder)(entity);
            string projectPath = entity.Project.OutputPath.Replace("\\", "/");
            string packagePath = entity.Project.PackagePath.Replace(".", "/");
            string projectName = entity.Project.Name;
            if (entity.Project.HasApi && template.InApiProject)
            {
                projectPath = projectPath.Replace($"{entity.Project.Name}-service", $"{entity.Project.Name}-api");
                projectName = Path.Combine(projectName, "api");
            }

            string outputPath =
                Path.Combine(
                    projectPath,
                    "main",
                    "java",
                    packagePath,
                    projectName,
                    folder);
            return outputPath;
        }

        private async Task GenerateCodeFile(Entity entity, Template template)
        {
            string content = await _templateEngine.Render(entity, template);
            var fileName = Handlebars.Compile(template.OutputName)(entity);
            string outputPath = MakeCodeFilePath(entity, template);
            FileHelper.CreateFile(outputPath, fileName, content);
        }
    }
}