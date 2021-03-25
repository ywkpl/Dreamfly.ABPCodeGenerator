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

        public ExcelExternalEntity(ImportExcelDto dto)
        {
            this._importExcelDto = dto;
        }

        protected override TableSetting ReadTableSetting()
        {
            using var stream = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            XSSFWorkbook xssfWorkbook = new XSSFWorkbook(stream);
            var iSheet = xssfWorkbook.GetSheetAt(_importExcelDto.TabIndex);

            var tableSetting = new TableSetting {Fields = new List<TableFieldSetting>()};
            for (int i = _importExcelDto.StartRow - 1; i < _importExcelDto.EndRow; i++)
            {
                var row = iSheet.GetRow(i);
                if (i == _importExcelDto.StartRow - 1) //首行
                {
                    tableSetting.Name = row.Cells[1].StringCellValue;
                    tableSetting.Desc = row.Cells[0].StringCellValue;
                }

                tableSetting.Fields.Add(new TableFieldSetting
                {
                    Name = row.Cells[3].StringCellValue,
                    Desc = row.Cells[2].StringCellValue,
                    Order = i - _importExcelDto.StartRow + 2
                });
            }

            return tableSetting;
        }
    }
}