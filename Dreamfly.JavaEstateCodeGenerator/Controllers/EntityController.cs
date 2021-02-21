using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Core;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Dreamfly.JavaEstateCodeGenerator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly Project _project;
        private readonly IProjectBuilder _projectBuilder;

        public EntityController(IOptionsSnapshot<Project> project, IProjectBuilder projectBuilder)
        {
            _project = project.Value;
            _projectBuilder = projectBuilder;
        }

        [HttpPost("Generate")]
        public async Task Generate(Entity entity)
        {
            //转换成Entity
            entity.Project = _project;

            //生成Code文件
            await _projectBuilder.Build(entity);
        }

        [HttpPost("Remove")]
        public async Task Remove(Entity entity)
        {
            //转换成Entity
            entity.Project = _project;

            //移除Code文件
            await _projectBuilder.Remove(entity);
        }

        [HttpPost("Import")]
        public Entity Import(ImportExcelDto dto)
        {
            return new ExcelHelper().ToEntity(dto);
        }
    }
}
