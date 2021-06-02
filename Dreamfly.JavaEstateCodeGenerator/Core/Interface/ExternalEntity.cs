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
        private const int CompanyOrder = 9990;
        private const int TenantOrder = 9999;

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
            int index = 10;
            foreach (var tableFieldSetting in _tableSetting.Fields)
            {
                if (IsFilterField(tableFieldSetting.Name)) continue;

                var fieldItem = GetFieldItem(tableFieldSetting, index);
                ResultEntity.EntityItems.Add(fieldItem);
                index += 10;
            }
        }

        private EntityItemDto GetFieldItem(TableFieldSetting tableField, int index)
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
                InResponse = false,
                InAllResponse = true,
                Order = tableField.Order ?? index
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
                Name = _tableSetting.Name.RemoveUnderLine(),
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
                        InResponse = false,
                        InAllResponse = false,
                        Order = TenantOrder
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
                    entityItem.InAllResponse = false;
                    entityItem.Order = TenantOrder;
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
                        InQuery = false,
                        InCreate = false,
                        InResponse = false,
                        InAllResponse = false,
                        Order = CompanyOrder
                    });
                }
                else
                {
                    entityItem.Type = "Long";
                    entityItem.Description = "公司編號";
                    entityItem.IsRequired = false;
                    entityItem.InQuery = false;
                    entityItem.InCreate = false;
                    entityItem.InResponse = false;
                    entityItem.InAllResponse = false;
                    entityItem.Order = CompanyOrder;
                }
            }
        }

        private void SetEntityItemDefaultValues(EntityItemDto itemDto)
        {
            if (itemDto.Name == "code" || itemDto.Name == "name" || itemDto.Name == "shortName")
            {
                itemDto.Length = 20;
                itemDto.InQuery = true;
                itemDto.InResponse = true;
            }

            if (itemDto.Name == "memo")
            {
                itemDto.Length = 2000;
            }

            if (itemDto.Name == "title")
            {
                itemDto.Length = 100;
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
                    itemDto.RelateEntity = itemDto.ColumnName.Replace("_Id", "").Replace("_","").ToPascalCase();
                    itemDto.RelateDirection = "Join";
                    itemDto.FetchType = "FetchType.LAZY";
                }
            }

            if (itemDto.ColumnName.EndsWith("Date"))
            {
                itemDto.Type = "Date";
                itemDto.InResponse = true;
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

            if (itemDto.ColumnName.EndsWith("Price"))
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
                itemDto.InResponse = true;
                itemDto.FetchType = "FetchType.LAZY";
            }
        }

        private bool IsFilterField(string field)
        {
            return FilterFields.Contains(field.ToLower());
        }

        private string ConvertFieldName(string field)
        {
            var name = field.RemoveUnderLine().ToCamelCase();
            if (name == "default")
            {
                name = "isDefault";
            }
            return name;
        }
    }
}