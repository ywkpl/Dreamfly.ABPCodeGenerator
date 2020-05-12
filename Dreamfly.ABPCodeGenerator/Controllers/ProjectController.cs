using System.IO;
using Dreamfly.ABPCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Dreamfly.ABPCodeGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            var content = JObject.FromObject(new {Project = project}).ToString();
            System.IO.File.WriteAllText(settingFilePath, content);
        }
    }
}