using System;
using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public abstract class ExternalEntity
    {
        protected EntityDto ResultEntity;
        protected List<string> FilterFields = new List<string>
        {
            "seq", "id", "adduser_id", "adddate", "moduser_id", "moddate"
        };
        private TableSetting _tableSetting;

        public virtual EntityDto ReadEntity()
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

        private EntityItemDto GetFieldItem(TableFieldSetting tableField)
        {
            string field = tableField.Name;
            var item = new EntityItemDto
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
            ResultEntity = new EntityDto
            {
                HasIHasCompany = true,
                HasIHasTenant = true,
                IsSync = false,
                TableName = _tableSetting.Name,
                Name = RemoveUnderLine(_tableSetting.Name),
                Description = _tableSetting.Desc,
                EntityItems = new List<EntityItemDto>()
            };
        }

        private void AddTenantField()
        {
            if (ResultEntity.HasIHasTenant)
            {
                var entityItem = ResultEntity.EntityItems.FirstOrDefault(t => t.Name == "tenantId");
                if (entityItem == null)
                {
                    ResultEntity.EntityItems.Add(new EntityItemDto
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
                    ResultEntity.EntityItems.Add(new EntityItemDto
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

        private void SetEntityItemDefaultValues(EntityItemDto itemDto)
        {
            if (itemDto.Name == "code" || itemDto.Name == "name" || itemDto.Name == "shortName")
            {
                itemDto.Length = 20;
                itemDto.InQuery = true;
            }

            if (itemDto.Name == "memo")
            {
                itemDto.Length = 1000;
            }

            if (itemDto.ColumnName.EndsWith("_Id"))
            {
                itemDto.Type = "Long";
                if (!itemDto.ColumnName.Contains("File") 
                    && !itemDto.ColumnName.ToLower().Equals("company_id") 
                    && !itemDto.ColumnName.ToLower().Equals("tenant_id"))
                {
                    itemDto.InQuery = true;
                    itemDto.RelateType = "ManyToOne";
                    itemDto.RelateEntity = itemDto.ColumnName.Replace("_Id", "").ToPascalCase();
                    itemDto.RelateDirection = "Join";
                }
            }

            if (itemDto.ColumnName.EndsWith("Date"))
            {
                itemDto.Type = "Date";
            }

            if (itemDto.ColumnName.EndsWith("Area"))
            {
                itemDto.Type = "BigDecimal";
                itemDto.Length = 18;
                itemDto.Fraction = 4;
            }

            if (itemDto.ColumnName.EndsWith("Amount"))
            {
                itemDto.Type = "BigDecimal";
                itemDto.Length = 18;
                itemDto.Fraction = 0;
            }

            if (itemDto.ColumnName.EndsWith("Qty"))
            {
                itemDto.Type = "Integer";
            }

            if (itemDto.ColumnName.StartsWith("Code_"))
            {
                itemDto.Type = "Long";
                itemDto.InQuery = true;
                itemDto.RelateType = "ManyToOne";
                itemDto.RelateEntity = "SysCode";
                itemDto.RelateDirection = "Join";
            }
        }

        private bool IsFilterField(string field)
        {
            return FilterFields.Contains(field.ToLower());
        }

        private string ConvertFieldName(string field)
        {
            return RemoveUnderLine(field).ToCamelCase();
        }

        private string RemoveUnderLine(string field)
        {
            return field.Replace("_", "");
        }
    }
}