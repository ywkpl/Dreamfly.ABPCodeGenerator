﻿using System.Collections.Generic;
using Dreamfly.JavaEstateCodeGenerator.Core.Impl;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Core;

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

        [HttpPost("Update")]
        public EntityDto Update(EntityDto entity)
        {
            return _entityPersistence.Update(entity);
        }

        [HttpPost("DeleteItems")]
        public void DeleteItems(List<int> itemIds)
        {
            _entityPersistence.DeleteItems(itemIds);
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

        [HttpGet("CreateSql")]
        public string CreateSql(string entityName)
        {
            EntityDto dto = _entityPersistence.Get(entityName);
            return new CreateSql(dto).ToSql();
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

        [HttpGet("SyncFields")]
        public EntityDto SyncFields(string tableName)
        {
            //讀取96設定
            //對比字段：
            //      新增：存在96中，不存在本地設定
            //      刪除：存在本地，不存在96中
            //      修改：新增+刪除
            //生成SQL
            //返回 實體數據+SQL
            return null;
        }
    }
}
