using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Contexts;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npoi.Mapper;
using NPOI.XSSF.UserModel;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class InitializeSql
    {
        private readonly Project _project;
        private readonly String filePath;
        private const string ExcelFilePath = "Excel\\InitializeData.xlsx";
//        private const int DefaultCompanyId = 1;
//        private const int DefaultTenantId = 1;

        public InitializeSql(Project project)
        {
            this._project = project;
            filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\Initilize.sql");

        }

        public void GeneratorCheckSetting()
        {

            var mapper = new Mapper(ExcelFilePath);
            var excelData = mapper
                .Take<CheckSettingDto>(1)
                .Where(t => t.Value != null && !String.IsNullOrEmpty(t.Value.Code) && t.Value.Code!="A19")
                //A19待修正長度，後緒再增加進來
                .Select(t => t.Value)
                .ToList();

            var checkSettings = excelData
                .GroupBy(t => new {t.Code, t.Name})
                .Select(t => new {t.Key.Code, t.Key.Name})
                .OrderBy(t => t.Code)
                .ToList();

            var checkSettingItems = excelData
                .GroupBy(t => new {t.Code, t.Name, t.DetailCode, t.DetailName})
                .Select(t => new {t.Key.Code, t.Key.Name, t.Key.DetailCode, t.Key.DetailName})
                .OrderBy(t => t.Code)
                .ThenBy(t => t.DetailCode)
                .ToList();

            StringBuilder sqlBuilder = new StringBuilder();
            int startId = 10, detailStartId = 10;
            checkSettings.ForEach(t =>
            {
                sqlBuilder.Append(
                    $"delete from CheckSetting_Item where CheckSetting_Id={startId} and Pid is not null;{Environment.NewLine}" +
                    $"delete from CheckSetting_Item where CheckSetting_Id={startId};{Environment.NewLine}");
                sqlBuilder.Append(
                    $"delete from CheckSetting where id={startId};{Environment.NewLine}");

                sqlBuilder.Append(
                    $"insert into CheckSetting(id, Code, Name, Memo) values ({startId}, '{t.Code}', '{t.Name}', '{t.Name}');{Environment.NewLine}");

                int masterId = startId;
                checkSettingItems
                    .Where(q => q.Code == t.Code)
                    .ForEach(item =>
                    {
                        int detailId = detailStartId;
                        sqlBuilder.Append(
                            $"insert into CheckSetting_Item(id, Code, Name, CheckSetting_Id, Pid, Memo) values ({detailId}, '{item.DetailCode}', '{item.DetailName}', {masterId}, null, '{item.DetailName}');{Environment.NewLine}");
                        
                        int childId = detailId * 100 + 10;
                        excelData.Where(w => w.Code == t.Code && w.DetailCode == item.DetailCode)
                            .ForEach(child =>
                            {
                                sqlBuilder.Append(
                                    $"insert into CheckSetting_Item(id, Code, Name, CheckSetting_Id, Pid, Memo) values ({childId}, '{child.ChildCode}', '{child.ChildName}', {masterId}, {detailId}, '{item.DetailName}');{Environment.NewLine}");
                                childId += 10;
                            });
                        detailStartId += 10;
                    });

                startId += 10;
            });

            File.WriteAllText(filePath, sqlBuilder.ToString(), Encoding.UTF8);
        }

        public void GeneratorFacilitySetting()
        {
//            using var stream = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
//            XSSFWorkbook xssfWorkbook = new XSSFWorkbook(stream);
//            var iSheet = xssfWorkbook.GetSheetAt(2);

            using var estateContext = new EstateContext();
            var test = ReadSysCodeFacilityType();
            SerilogHelper.Instance.Error("count:{count}", test.Count());
        }

        private List<SysCodeDto> ReadSysCodeFacilityType()
        {
            const string codeString = "FacilityType";
            using var estateContext = new EstateContext();
            var parentCode = estateContext.SysCodes
                .Include(t=>t.InversePidNavigation)
                .SingleOrDefault(t => t.Code == codeString && t.Pid == null);
            if (parentCode != null)
            {
                parentCode.InversePidNavigation.ForEach(t =>
                {
                    SerilogHelper.Instance.Error("count:{count}", t.InversePidNavigation.Count());
                });
            }

            return parentCode?.InversePidNavigation
                .Select(t => new SysCodeDto {Pid = t.Pid, PidCode = t.PidNavigation.Code, Id = t.Id, Code = t.Code})
                .ToList();
        }
    }
}