using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Npoi.Mapper;
using NPOI.XSSF.UserModel;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class InitializeSql
    {
        private readonly Project _project;
        private readonly String filePath;
        private const string ExcelFilePath = "Excel\\InitializeData.xlsx";

        public InitializeSql(Project project)
        {
            this._project = project;
            filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\Initilize.sql");

        }

        public void GeneratorCheckSetting()
        {
            
            var mapper=new Mapper(ExcelFilePath);
            var excelData = mapper
                .Take<CheckSettingDto>(1)
                .Where(t=>t.Value!=null && !String.IsNullOrEmpty(t.Value.Code))
                .Select(t=>t.Value)
                .ToList();

            var checkSettings = excelData
                .GroupBy(t => new {t.Code, t.Name})
                .Select(t => new {t.Key.Code, t.Key.Name})
                .OrderBy(t=>t.Code)
                .ToList();

            var checkSettingItems= excelData
                .GroupBy(t => new { t.Code, t.Name, t.DetailCode, t.DetailName })
                .Select(t => new { t.Key.Code, t.Key.Name, t.Key.DetailCode, t.Key.DetailName })
                .OrderBy(t=>t.Code)
                .ThenBy(t=>t.DetailCode)
                .ToList();

            StringBuilder sqlBuilder=new StringBuilder();
            int startId = 10;
            checkSettings.ForEach(t =>
            {
                int detailStartId = 10;
                int id = startId;
                sqlBuilder.Append(
                    $"insert into CheckSetting(id, Code, Name, Company_Id) values ({startId}, '{t.Code}', '{t.Name}',1);");
                sqlBuilder.Append(Environment.NewLine);

                checkSettingItems
                    .Where(q=>q.Code==t.Code)
                    .ForEach(item =>
                    {
                        sqlBuilder.Append(
                            $"insert into CheckSetting_Item(id, Code, Name, CheckSetting_Id, Pid) values ({detailStartId}, '{item.DetailCode}', '{item.DetailName}', {id}, null);");
                        sqlBuilder.Append(Environment.NewLine);

                        int detailId = detailStartId;
                        int childId = detailStartId * 100 + 10;
                        excelData.Where(w=>w.Code==t.Code && w.DetailCode==item.DetailCode)
                            .ForEach(child =>
                            {
                                sqlBuilder.Append(
                                    $"insert into CheckSetting_Item(id, Code, Name, CheckSetting_Id, Pid) values ({childId}, '{child.ChildCode}', '{child.ChildName}', {id}, {detailId});");
                                sqlBuilder.Append(Environment.NewLine);
                                childId += 10;
                            });
                        detailStartId += 10;
                    });

                startId += 10;
            });
            SerilogHelper.Instance.Error("sql:" + sqlBuilder.ToString());
            //
            // using var stream = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            // XSSFWorkbook xssfWorkbook = new XSSFWorkbook(stream);
            // var iSheet = xssfWorkbook.GetSheetAt(1);
            // var excelData = new List<CheckSettingDto>();
            // for (int i = 1; i < _importExcelDto.EndRow; i++)
            // {
            // }
        }
    }
}