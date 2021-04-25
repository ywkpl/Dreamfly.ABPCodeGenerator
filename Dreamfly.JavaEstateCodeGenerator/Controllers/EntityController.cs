﻿using Dreamfly.JavaEstateCodeGenerator.Core.Impl;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Dreamfly.JavaEstateCodeGenerator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly Project _project;
        private readonly IProjectBuilder _projectBuilder;
        private readonly IEntityPersistence _entityPersistence;

        public EntityController(IOptionsSnapshot<Project> project, IProjectBuilder projectBuilder, IEntityPersistence entityPersistence)
        {
            _project = project.Value;
            _projectBuilder = projectBuilder;
            _entityPersistence = entityPersistence;
        }

        [HttpPost("Generate")]
        public async Task Generate(EntityDto entity)
        {
            _entityPersistence.Save(entity);
            //转换成Entity
            entity.Project = _project;

            //生成Code文件
            await _projectBuilder.Build(entity);
        }

        [HttpPost("Save")]
        public void Save(EntityDto entity)
        {
            _entityPersistence.Save(entity);
        }

        [HttpGet("Get")]
        public EntityDto Get(string entityName)
        {
            return _entityPersistence.Get(entityName);
        }

        [HttpPost("Remove")]
        public async Task Remove(EntityDto entity)
        {
            //转换成Entity
            entity.Project = _project;

            //移除Code文件
            await _projectBuilder.Remove(entity);
        }

        [HttpPost("ImportFromExcel")]
        public EntityDto ImportFromExcel(ImportExcelDto dto)
        {
            return new ExcelExternalEntity(dto).ReadEntity();
        }

        [HttpGet("ImportFromDB")]
        public EntityDto ImportFromDb(string tableName)
        {
            return new DBExternalEntity(tableName).ReadEntity();
        }
    }
}
