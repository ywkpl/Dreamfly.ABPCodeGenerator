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

        private StringBuilder sqlBuilder;
//        private const int DefaultCompanyId = 1;
//        private const int DefaultTenantId = 1;

        public InitializeSql(Project project)
        {
            this._project = project;
            filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\Initilize.sql");
            sqlBuilder = new StringBuilder();
        }

        private void GeneratorCheckSetting()
        {
            var mapper = new Mapper(ExcelFilePath);
            var excelData = mapper
                .Take<CheckSettingDto>(1)
                .Where(t => t.Value != null && !String.IsNullOrEmpty(t.Value.Code) && t.Value.Code != "A19")
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

            int startId = 10, detailStartId = 10;
            sqlBuilder.Append($"{Environment.NewLine}{Environment.NewLine}#####=== CheckSetting ===#####{Environment.NewLine}");
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
        }

        public void GenerateSql()
        {
            GeneratorCheckSetting();
            GeneratorFacilitySetting();
            File.WriteAllText(filePath, sqlBuilder.ToString(), Encoding.UTF8);
        }

        private void GeneratorFacilitySetting()
        {
            var mapper = new Mapper(ExcelFilePath);
            var excelFacilitySetting = mapper
                .Take<FacilitySettingDto>(2)
                .Where(t => t.Value != null && !string.IsNullOrEmpty(t.Value.CodeFacilityType))
                .Select(t => t.Value)
                .ToList();

            var codeFacilityType = ReadSysCodeFacilityType();
            excelFacilitySetting.ForEach(t =>
            {
                var sysCode = codeFacilityType
                    .FirstOrDefault(q => q.PidCode == t.CodeFacilityType && q.Code == t.CodeFacilityType2);
                if (sysCode != null)
                {
                    t.CodeFacilityTypeId = sysCode.Pid;
                    t.CodeFacilityType2Id = sysCode.Id;
                }
            });

            int startId = 10;
            sqlBuilder.Append($"{Environment.NewLine}{Environment.NewLine}#####=== FacilitySetting ===#####{Environment.NewLine}");
            excelFacilitySetting.Where(t => t.CodeFacilityTypeId.HasValue).ForEach(t =>
            {
                sqlBuilder.Append(
                    $"delete from FacilitySetting where id={startId};{Environment.NewLine}");

                sqlBuilder.Append(
                    $"insert into FacilitySetting(id, Code, Name, Code_FacilityType, Code_FacilityType2, Specification) " +
                    $"values ({startId}, '{t.Code}', '{t.Name}', {t.CodeFacilityTypeId}, {t.CodeFacilityType2Id}, '{t.Specification}');{Environment.NewLine}");

                startId += 10;
            });
        }

        private List<SysCodeDto> ReadSysCodeFacilityType()
        {
            const string codeString = "FacilityType";
            using var estateContext = new EstateContext();
            var parentCode = estateContext.SysCodes
                .Include(t => t.InversePidNavigation)
                .ThenInclude(t => t.InversePidNavigation)
                .SingleOrDefault(t => t.Code == codeString && t.Pid == null);

            return parentCode?.InversePidNavigation.SelectMany(t =>
                    
                t.InversePidNavigation
                    .Select(z => new SysCodeDto
                    {
                        Pid = z.Pid,
                        PidCode = z.PidNavigation.Code,
                        Id = z.Id,
                        Code = z.Code
                    }))
                .ToList();
        }
    }
}