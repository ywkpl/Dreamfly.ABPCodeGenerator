﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Core.Interface;
using Dreamfly.ABPCodeGenerator.Helper;
using Dreamfly.ABPCodeGenerator.Models;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class ProjectBuilder:IProjectBuilder
    {
        private readonly ITemplateEngine _templateEngine;
        private readonly ILogger<ProjectBuilder> _logger;

        public ProjectBuilder(ITemplateEngine templateEngine, ILogger<ProjectBuilder> logger)
        {
            _templateEngine = templateEngine;
            _logger = logger;
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
                var insertCodes = GetInsertCodeBases(entity);
                insertCodes.ForEach(p => p.Remove());
            });


        }

        private void RemoveCodeFile(Entity entity, Template template)
        {
            var fileName = Handlebars.Compile(template.OutputName)(entity);
            var folder = Handlebars.Compile(template.OutputFolder)(entity);
            string apiOutputPath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src", folder);

            FileHelper.DeleteFile(apiOutputPath, fileName);
        }

        private void InsertCodeToFile(Entity entity)
        {
            var insertCodes = GetInsertCodeBases(entity);
            insertCodes.ForEach(p => p.Remove());
            insertCodes.ForEach(p=>p.Insert());
        }

        private static List<InsertCodeToFileBase> GetInsertCodeBases(Entity entity)
        {
            List<InsertCodeToFileBase> insertCodes = new List<InsertCodeToFileBase>
            {
                new ToPermissionNames(entity),
                new InsertLanguageXml(entity),
                new ToDbContext(entity),
                new ToAuthorizationProvider(entity)
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

                InsertCodeToFile(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task GenerateCodeFile(Entity entity, Template template)
        {
            string content = await _templateEngine.Render(entity, template);
            var fileName = Handlebars.Compile(template.OutputName)(entity);
            var folder = Handlebars.Compile(template.OutputFolder)(entity);
            string apiOutputPath = Path.Combine(entity.Project.OutputPath, "aspnet-core", "src", folder);

            FileHelper.CreateFile(apiOutputPath, fileName, content);
        }
    }
}