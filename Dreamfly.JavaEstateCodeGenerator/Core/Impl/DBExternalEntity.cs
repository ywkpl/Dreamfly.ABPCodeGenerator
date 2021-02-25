using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl
{
    public class DBExternalEntity:ExternalEntity
    {
        private readonly string _tableName;
        private TableFieldDto _tableField;
        private List<TableFieldDto> _tableFields;

        public DBExternalEntity(string tableName)
        {
            this._tableName = tableName;
        }

        public override Entity ReadEntity()
        {
            ReadFieldSettings();
            if (_tableFields.Count == 0)
            {
                return null;
            }
            MakeResultEntity();
            AddFields();
            AddCompanyField();
            AddTenantField();
            return ResultEntity;
        }
        
        private void ReadFieldSettings()
        {
            using var dbContext = new ADJUSTDBContext();
            _tableFields = dbContext.HousingFieldSettings1
                .Join(dbContext.HousingTableSettings,
                    d => d.TablesettingsId,
                    m => m.Id,
                    (d, m) => new {d, m})
                .Where(t => t.d.Tablename == _tableName)
                .OrderBy(t => t.d.Seq)
                .Select(t => new TableFieldDto
                {
                    TableName = t.d.Tablename,
                    TableDesc = t.m.Description,
                    FieldName = t.d.Fieldname,
                    FieldDesc = t.d.Fielddesc
                }).ToList();
        }

        protected override void AddFields()
        {
            for (int i = 0; i < _tableFields.Count; i++)
            {
                _tableField = _tableFields[i];
                if (i == 0)
                {
                    ResultEntity.TableName = _tableField.TableName;
                    ResultEntity.Name = RemoveUnderLine(ResultEntity.TableName);
                    ResultEntity.Description = _tableField.TableDesc;
                }
                if (IsFilterField(_tableField.FieldName)) continue;

                var fieldItem = GetFieldItem();
                ResultEntity.EntityItems.Add(fieldItem);
            }
        }

        private EntityItem GetFieldItem()
        {
            string field = _tableField.FieldName;
            var item = new EntityItem
            {
                Name = ConvertFieldName(field),
                ColumnName = field,
                Description = _tableField.FieldDesc,
                IsRequired = false,
                Type = "String",
                InQuery = false,
                InCreate = true,
                InResponse = true
            };
            SetEntityItemDefaultValues(item);
            return item;
        }
    }
}