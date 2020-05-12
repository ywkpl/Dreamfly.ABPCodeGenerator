using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Core.Interface;
using Dreamfly.ABPCodeGenerator.Helper;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class ProjectBuilder:IProjectBuilder
    {
        private readonly ITemplateEngine _templateEngine;
        public ProjectBuilder(ITemplateEngine templateEngine)
        {
            _templateEngine = templateEngine;
        }

        public async Task<string> Build(Entity entity)
        {
            List<Template> templates = entity.Project.BuildTask.Templates.Where(t => t.IsExecute).ToList();

            foreach (var template in templates)
            {
                string content = await _templateEngine.Render(entity);

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), entity.Project.OutputPath,
                    template.Output.Folder);

                FileHelper.CreateFile(outputPath, template.Output.Name, content);
            }

            return string.Empty;
        }

        public void IncludeToProject()
        {
            throw new System.NotImplementedException();
        }
    }
}