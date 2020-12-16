using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Dreamfly.JavaEstateCodeGenerator.Controllers
{
    [Route("api/[controller]")]
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
    }
}
