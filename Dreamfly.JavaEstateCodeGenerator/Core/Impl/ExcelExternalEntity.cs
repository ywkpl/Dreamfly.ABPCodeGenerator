using System.Collections.Generic;
using System.IO;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl
{
    public class ExcelExternalEntity: ExternalEntity
    {
        private readonly ImportExcelDto _importExcelDto;
        private const string ExcelFilePath = "Excel\\table.xlsx";
        private ISheet _iSheet;
        private IRow _row;

        public ExcelExternalEntity(ImportExcelDto dto)
        {
            this._importExcelDto = dto;
        }

        public override Entity ReadEntity()
        {
            using var stream = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            XSSFWorkbook xssfWorkbook = new XSSFWorkbook(stream);
            _iSheet = xssfWorkbook.GetSheetAt(_importExcelDto.TabIndex);

            MakeResultEntity();
            AddFields();
            AddCompanyField();
            AddTenantField();
            return ResultEntity;
        }

        protected override void AddFields()
        {
            for (int i = _importExcelDto.StartRow - 1; i < _importExcelDto.EndRow; i++)
            {
                _row = _iSheet.GetRow(i);
                if (i == _importExcelDto.StartRow - 1) //首行
                {
                    ResultEntity.TableName = _row.Cells[1].StringCellValue;
                    ResultEntity.Name = RemoveUnderLine(ResultEntity.TableName);
                    ResultEntity.Description = _row.Cells[0].StringCellValue;
                }
                if(IsFilterField(_row.Cells[3].StringCellValue)) continue;
                var fieldItem = GetFieldItem();
                ResultEntity.EntityItems.Add(fieldItem);
            }
        }

        private EntityItem GetFieldItem()
        {
            string field = _row.Cells[3].StringCellValue;
            var item = new EntityItem
            {
                Name = ConvertFieldName(field),
                ColumnName = field,
                Description = _row.Cells[2].StringCellValue,
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