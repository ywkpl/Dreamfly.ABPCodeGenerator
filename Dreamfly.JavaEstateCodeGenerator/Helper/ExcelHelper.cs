using System;
using System.Collections.Generic;
using System.IO;
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
                EntityItems = new List<EntityItem>()
            };

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

                entity.EntityItems.Add(new EntityItem
                {
                    Name = convertedField,
                    ColumnName = field,
                    Description = row.Cells[2].StringCellValue,
                    IsRequired = false,
                    Type = "String",
                    InQuery = false,
                    InCreate = true,
                    InResponse = true,
                    IsSync = false
                });
            }

            return entity;
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