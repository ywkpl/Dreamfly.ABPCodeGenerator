using Dreamfly.JavaEstateCodeGenerator.Core;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.SqliteDbModels;
using Mapster;

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

        [HttpPost("UpdateCodeKeyValues")]
        public void UpdateCodeKeyValues()
        {
            new CodeSql(_project).UpdateSqliteCodeKeyValues();
        }

        [HttpPost("InitializeSql")]
        public void InitializeSql()
        {
            new InitializeSql(_project).GenerateSql();
        }


        //        [HttpPost("ToDb")]
        //        public void SaveJsonToDb()
        //        {
        //            var entityDtos = JsonDataHelper.ReadEntities();
        //            using (var dbContext = new SettingContext())
        //            {
        //                var entities = entityDtos.AsQueryable().ProjectToType<Entity>();
        //                dbContext.Entity.AddRange(entities);
        //                dbContext.SaveChanges();
        //            }
        //        }
    }
}
