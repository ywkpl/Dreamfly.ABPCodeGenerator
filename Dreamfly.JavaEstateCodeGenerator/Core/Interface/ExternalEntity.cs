using System;
using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public abstract class ExternalEntity
    {
        protected Entity ResultEntity;
        protected List<string> FilterFields = new List<string>
        {
            "seq", "id", "adduser_id", "adddate", "moduser_id", "moddate"
        };
        private TableSetting _tableSetting;

        public virtual Entity ReadEntity()
        {
            _tableSetting = ReadTableSetting();
            if (IsNullOrEmpty()) return null;

            MakeResultEntity();
            AddFields();
            AddCompanyField();
            AddTenantField();
            return ResultEntity;
        }

        private bool IsNullOrEmpty()
        {
            return _tableSetting?.Fields == null || _tableSetting.Fields.Count == 0;
        }

        protected abstract TableSetting ReadTableSetting();

        private void AddFields()
        {
            for (int i = 0; i < _tableSetting.Fields.Count; i++)
            {
                var tableFieldSetting = _tableSetting.Fields[i];
                if (IsFilterField(tableFieldSetting.Name)) continue;

                var fieldItem = GetFieldItem(tableFieldSetting);
                ResultEntity.EntityItems.Add(fieldItem);
            }
        }

        private EntityItem GetFieldItem(TableFieldSetting tableField)
        {
            string field = tableField.Name;
            var item = new EntityItem
            {
                Name = ConvertFieldName(field),
                ColumnName = field,
                Description = tableField.Desc,
                IsRequired = false,
                Type = "String",
                InQuery = false,
                InCreate = true,
                InResponse = true
            };
            SetEntityItemDefaultValues(item);
            return item;
        }

        private void MakeResultEntity()
        {
            ResultEntity = new Entity
            {
                HasIHasCompany = true,
                HasIHasTenant = true,
                IsSync = false,
                TableName = _tableSetting.Name,
                Name = RemoveUnderLine(_tableSetting.Name),
                Description = _tableSetting.Desc,
                EntityItems = new List<EntityItem>()
            };
        }

        private void AddTenantField()
        {
            if (ResultEntity.HasIHasTenant)
            {
                var entityItem = ResultEntity.EntityItems.FirstOrDefault(t => t.Name == "tenantId");
                if (entityItem == null)
                {
                    ResultEntity.EntityItems.Add(new EntityItem
                    {
                        Name = "tenantId",
                        ColumnName = "TenantId",
                        Description = "集團編號",
                        IsRequired = false,
                        Type = "Long",
                        InQuery = false,
                        InCreate = false,
                        InResponse = false
                    });
                }
                else
                {
                    entityItem.Type = "Long";
                    entityItem.Description = "集團編號";
                    entityItem.IsRequired = false;
                    entityItem.InQuery = false;
                    entityItem.InCreate = false;
                    entityItem.InResponse = false;
                }
            }
        }

        private void AddCompanyField()
        {
            if (ResultEntity.HasIHasCompany)
            {
                var entityItem = ResultEntity.EntityItems.FirstOrDefault(t => t.Name == "companyId");
                if (entityItem == null)
                {
                    ResultEntity.EntityItems.Add(new EntityItem
                    {
                        Name = "companyId",
                        ColumnName = "CompanyId",
                        Description = "公司編號",
                        IsRequired = false,
                        Type = "Long",
                        InQuery = true,
                        InCreate = false,
                        InResponse = false
                    });
                }
                else
                {
                    entityItem.Type = "Long";
                    entityItem.Description = "公司編號";
                    entityItem.IsRequired = false;
                    entityItem.InQuery = true;
                    entityItem.InCreate = false;
                    entityItem.InResponse = false;
                }
            }
        }

        private void SetEntityItemDefaultValues(EntityItem item)
        {
            if (item.Name == "code" || item.Name == "name")
            {
                item.Length = 20;
                item.InQuery = true;
            }

            if (item.Name == "memo")
            {
                item.Length = 1000;
            }

            if (item.ColumnName.EndsWith("_Id"))
            {
                item.Type = "Long";
                if (!item.ColumnName.Contains("File"))
                {
                    item.InQuery = true;
                }
            }

            if (item.ColumnName.StartsWith("Code_"))
            {
                item.Type = "Long";
                item.InQuery = true;
            }
        }

        private bool IsFilterField(string field)
        {
            return FilterFields.Contains(field.ToLower());
        }

        private string ConvertFieldName(string field)
        {
            return ToCamelCase(RemoveUnderLine(field));
        }

        private string RemoveUnderLine(string field)
        {
            return field.Replace("_", "");
        }

        private string ToCamelCase(string field)
        {
            if (String.IsNullOrWhiteSpace(field))
            {
                return String.Empty;
            }
            return char.ToLowerInvariant(field[0]) + field.Substring(1);
        }
    }
}