using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Core.Impl;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Core;
using Dreamfly.JavaEstateCodeGenerator.Helper;

namespace Dreamfly.JavaEstateCodeGenerator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly Project _project;
        private readonly IProjectBuilder _projectBuilder;
        private readonly IEntityPersistence _entityPersistence;

        public EntityController(IOptionsSnapshot<Project> project, IProjectBuilder projectBuilder,
            IEntityPersistence entityPersistence)
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

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SavedDto Add(EntityDto entity)
        {
            var sql = GetCreateSql(entity);
            var savedEntity = _entityPersistence.Add(entity);

            return new SavedDto
            {
                Sql = sql,
                EntityDto = savedEntity
            };
        }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SavedDto Update(EntityDto entity)
        {
            var sql = GetUpdateSql(entity);
            var savedEntity = _entityPersistence.Update(entity);

            return new SavedDto
            {
                Sql = sql,
                EntityDto = savedEntity
            };
        }

        private string GetUpdateSql(EntityDto entity)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            
            var addFields = entity.EntityItems
                .Where(t => t.Id == null && t.Type != "Set")
                .ToList();

            //讀取現有實體
            var dbEntityDto = _entityPersistence.Get(entity.Id.Value);
            //刪除字段[原實體存在, 提交實體中不存在]
            var dbFields = dbEntityDto.EntityItems.Select(t => t.Id).ToList();
            var fields = entity.EntityItems.Select(t => t.Id).ToList();
            var deleteFields = dbFields.Except(fields);
            var dropFields = dbEntityDto.EntityItems
                .Where(t => deleteFields.Contains(t.Id) && t.Type != "Set")
                .ToList();

            if (addFields.Any() || dropFields.Any())
            {
                sqlBuilder.Append($"alter table `{entity.TableName}`");
                //新增字段
                addFields.ForEach(p => sqlBuilder.Append(GetAddFieldSql(p)));
                dropFields.ForEach(p => sqlBuilder.Append(GetDropFieldSql(p)));

                //去除最後","号,添加";"
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
                sqlBuilder.Append(";");
            }

            return sqlBuilder.ToString();
        }

        private string GetCreateSql(EntityDto entity)
        {
            if (!entity.EntityItems.Any())
            {
                throw  new Exception("無明細數據");
            }
            StringBuilder sqlBuilder = new StringBuilder($"create table if not exists `{entity.TableName}` (");
            sqlBuilder.Append($"{Environment.NewLine}\tid bigint not null,");
            entity.EntityItems.ForEach(p =>  sqlBuilder.Append(GetCreateFieldSql(p)));
            //添加固定欄位[新增，刪除，修改等]
            sqlBuilder.Append(FullAuditFieldSql);
            sqlBuilder.Append($"{Environment.NewLine}\tprimary key (id)");
            sqlBuilder.Append(") engine = InnoDB;");
            return sqlBuilder.ToString();
        }

        private string FullAuditFieldSql =>
            $"{Environment.NewLine}\tcreationTime datetime comment '新增時間',"
            +$"{Environment.NewLine}\tcreatorUserId bigint comment '新增人員',"
            + $"{Environment.NewLine}\tlastModificationTime datetime comment '最近修改時間',"
            + $"{Environment.NewLine}\tlastModifierUserId bigint comment '最近修改人員',"
            + $"{Environment.NewLine}\tdeleterUserId bigint comment '刪除人員',"
            + $"{Environment.NewLine}\tdeletionTime datetime comment '刪除時間',"
            + $"{Environment.NewLine}\tisDeleted bit default 0 comment '刪除否',";

        [HttpPost("SaveTest")]
        public SavedDto SaveTest(EntityDto entity)
        {
            return entity.Id == null ? Add(entity) : Update(entity);
        }

        private string GetDropFieldSql(EntityItemDto item)
        {
            return $"{Environment.NewLine}\tdrop column if exists `{item.FieldName}`,";
        }

        private string GetCreateFieldSql(EntityItemDto item)
        {
            string fieldSql = GetFieldSql(item);
            return $"{Environment.NewLine}\t{fieldSql},";
        }

        private string GetAddFieldSql(EntityItemDto item)
        {
            string fieldSql= GetFieldSql(item);
            return $"{Environment.NewLine}\tadd column if not exists {fieldSql},";
        }

        private string GetFieldSql(EntityItemDto item)
        {
            StringBuilder fieldBuilder = new StringBuilder($"`{item.FieldName}`");
            DataType dataType = item.Type.ToEnum<DataType>(DataType.String);
            switch (dataType)
            {
                case DataType.String:
                    fieldBuilder.Append($" varchar({item.Length})");
                    break;
                case DataType.Long:
                    fieldBuilder.Append(" bigint");
                    break;
                case DataType.Integer:
                    fieldBuilder.Append(" integer");
                    break;
                case DataType.Boolean:
                    fieldBuilder.Append(" bit default 0");
                    break;
                case DataType.Date:
                    fieldBuilder.Append(" datetime");
                    break;
                case DataType.Float:
                    fieldBuilder.Append(" float");
                    break;
                case DataType.Json:
                    fieldBuilder.Append(" json");
                    break;
                case DataType.BigDecimal:
                    fieldBuilder.Append($" decimal({item.Length},{item.Fraction})");
                    break;
                case DataType.Text:
                    fieldBuilder.Append(" text");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (item.IsRequired)
            {
                fieldBuilder.Append(" not null");
            }

            fieldBuilder.Append($" comment '{item.Description}'");
            return fieldBuilder.ToString();
        }

        [HttpGet("SyncFields")]
        public EntityDto SyncFields(string tableName)
        {
            //讀取96設定
            var dbEntityDto = new DBExternalEntity(tableName).ReadEntity();
            //讀取本地設定
            var entityDto = _entityPersistence.Get(tableName.RemoveUnderLine());

            var dbFields = dbEntityDto.EntityItems.Select(t =>t.ColumnName).ToList();
            var fields = entityDto.EntityItems.Select(t => t.ColumnName).ToList();
            //新增字段【96中有，本地沒有】
            var addFields = dbFields.Except(fields);
            //刪除字段【本地有，96中沒有】
            var deleteFields = fields.Except(dbFields);

            var addEntityItmes = dbEntityDto.EntityItems
                .Where(t => addFields.Contains(t.ColumnName))
                .OrderBy(t=>t.Order)
                .ToList();

            var deleteEntityItmes = dbEntityDto.EntityItems
                .Where(t => deleteFields.Contains(t.ColumnName))
                .ToList();
            deleteEntityItmes.ForEach(t =>  entityDto.EntityItems.Remove(t) );
            entityDto.EntityItems.AddRange(addEntityItmes);

            //更新頭檔
            entityDto.Description = dbEntityDto.Description;
            //更新排序
            entityDto.EntityItems.ForEach(p =>
            {
                var item = dbEntityDto.EntityItems.FirstOrDefault(t => t.ColumnName == p.ColumnName);
                if (item != null)
                {
                    p.Order = item.Order;
                }
            });

            return entityDto;
        }
    }
}