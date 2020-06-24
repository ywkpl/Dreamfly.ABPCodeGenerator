using System;
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

            InsertCodeToFile(entity);
        }

        private void InsertCodeToFile(Entity entity)
        {
            List<InsertCodeToFileBase> insertCodes = new List<InsertCodeToFileBase>
            {
                new ToPermissionNames(entity),
                new InsertLanguageXml(entity),
                new ToDbContext(entity),
                new ToAuthorizationProvider(entity)
            };

            insertCodes.ForEach(p => p.Remove());
            insertCodes.ForEach(p=>p.Insert());
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