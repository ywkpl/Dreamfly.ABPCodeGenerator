using Dreamfly.JavaEstateCodeGenerator.Core;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Dreamfly.JavaEstateCodeGenerator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly Project _project;

        public ProjectController(IOptionsSnapshot<Project> project)
        {
            _project = project.Value;
        }

        [HttpGet]
        public Project Get()
        {
            return this._project;
        }

        [HttpPost]
        public void Post(Project project)
        {
            var settingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Project.json");
            var content = JObject.FromObject(new { Project = project }).ToString();
            System.IO.File.WriteAllText(settingFilePath, content);
        }

        [HttpPost("GeneratorCodeSql")]
        public void GeneratorCodeSql()
        {
            new CodeSql(_project).GeneratorFile();
        }
    }
}
