using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class ExcelHelper
    {
        private List<String> filterFields = new List<string>
        {
            "seq", "id", "adduser_id", "adddate", "moduser_id", "moddate"
        };

        public Entity ToEntity(ImportExcelDto dto)
        {
            using var stream = new FileStream("Excel\\table.xlsx", FileMode.Open);
            XSSFWorkbook xssfWorkbook = new XSSFWorkbook(stream);
            ISheet iSheet = xssfWorkbook.GetSheetAt(dto.TabIndex);

            Entity entity = new Entity
            {
                HasIHasCompany = true,
                HasIHasTenant = true,
                IsSync = false,
                EntityItems = new List<EntityItem>()
            };

            AddFieldItems(dto, iSheet, entity);
            AddCompanyItem(entity);
            AddTenantItem(entity);
            return entity;
        }

        private void AddFieldItems(ImportExcelDto dto, ISheet iSheet, Entity entity)
        {
            for (int i = dto.StartRow - 1; i < dto.EndRow; i++)
            {
                var row = iSheet.GetRow(i);
                if (i == dto.StartRow - 1) //首行
                {
                    entity.TableName = row.Cells[1].StringCellValue;
                    entity.Name = RemoveUnderLine(entity.TableName);
                    entity.Description = row.Cells[0].StringCellValue;
                }

                string field = row.Cells[3].StringCellValue;
                if (IsFilterField(field))
                {
                    continue;
                }

                string convertedField = ConvertFieldName(field);

                var item = new EntityItem
                {
                    Name = convertedField,
                    ColumnName = field,
                    Description = row.Cells[2].StringCellValue,
                    IsRequired = false,
                    Type = "String",
                    InQuery = false,
                    InCreate = true,
                    InResponse = true
                };

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

                entity.EntityItems.Add(item);
            }
        }

        private void AddTenantItem(Entity entity)
        {
            if (entity.HasIHasTenant)
            {
                var entityItem = entity.EntityItems.FirstOrDefault(t => t.Name == "tenantId");
                if (entityItem == null)
                {
                    entity.EntityItems.Add(new EntityItem
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

        private void AddCompanyItem(Entity entity)
        {
            if (entity.HasIHasCompany)
            {
                var entityItem = entity.EntityItems.FirstOrDefault(t => t.Name == "companyId");
                if (entityItem == null)
                {
                    entity.EntityItems.Add(new EntityItem
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

        private bool IsFilterField(string field)
        {
            return filterFields.Contains(field.ToLower());
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
            return char.ToLowerInvariant(field[0]) + field.Substring(1);
        }
    }
}