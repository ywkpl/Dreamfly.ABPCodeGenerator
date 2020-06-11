﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Core.Interface;
using Dreamfly.ABPCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dreamfly.ABPCodeGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntityController : ControllerBase
    {
        private readonly Project _project;
        private readonly IProjectBuilder _projectBuilder;
        
        public EntityController(IOptionsSnapshot<Project> project, IProjectBuilder projectBuilder)
        {
            _project = project.Value;
            _projectBuilder = projectBuilder;
        }

        [HttpPost]
        public async Task Post(Entity entity)
        {
            //转换成Entity
            entity.Project = _project;

            //生成Code文件
            await _projectBuilder.Build(entity);
        }
    }
}
