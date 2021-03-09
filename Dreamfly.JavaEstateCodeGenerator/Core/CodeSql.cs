using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Dreamfly.JavaEstateCodeGenerator.SqliteDbModels;
using Mapster;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class CodeSql
    {
        private readonly Project _project;
        private const long StartId = 19000;
        private const long Interval = 1000;
        private StringBuilder _sqlBuilder;

        public CodeSql(Project project)
        {
            this._project = project;
        }

        public void GeneratorFile()
        {
            ValidateRepeatCode();
            string sqlString = GeneratorSqlString();
            Save(sqlString);
        }

        private void Save(string sqlString)
        {
            String filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\SysCode.sql");
            File.WriteAllText(filePath, sqlString, Encoding.UTF8);
        }

        private void ValidateRepeatCode()
        {
            using var context=new ADJUSTDBContext();
            var repeatCodes= context.HousingFieldSettings1
                .Where(t => (t.Code != null && t.Code != "")
                            && (t.Selector != null && t.Selector != "")
                            && (t.Code != null && t.Code != "")
                )
                .GroupBy(t => t.Code)
                .Select(g => new {Code = g.Key, Count = g.Count()})
                .Where(t => t.Count > 1)
                .ToList();
            if (repeatCodes.Count > 0)
            {
                var codeMessage = String.Join(",", repeatCodes.Select(t => t.Code + " 數量:" + t.Count));
                SerilogHelper.Instance.Error("發現重複Code:" + codeMessage);
                throw new Exception("發現重複Code");
            }
        }

        private String GeneratorSqlString()
        {
            var items = ReadCodeItems();
            return GeneratorSqlBuilder(items).ToString();
        }

        private long GetStartCodeId()
        {
            using var context = new SettingContext();
            var maxCodeId = context.CodeTrack
                .Select(t => t.SysCodeId)
                .AsEnumerable()
                .DefaultIfEmpty(StartId)
                .Max();
            return maxCodeId + Interval;
        }

        private CodeTrack GetCodeTrackByCode(String code)
        {
            using var context = new SettingContext();
            return context.CodeTrack.SingleOrDefault(t => t.Code == code);
        }

//        private void InsertCodeTrack(CodeTrack codeTrack)
//        {
//            using var context = new SettingContext();
//            context.Add(codeTrack);
//            context.SaveChanges();
//        }

        private void InsertCodeTracks(List<CodeTrack> codeTracks)
        {
            using var context = new SettingContext();
            using var tran = context.Database.BeginTransaction();
            try
            {
                context.AddRange(codeTracks);
                context.SaveChanges();
                tran.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SerilogHelper.Instance.Error(e, "批次插入代码不成功");
            }
        }

        private StringBuilder GeneratorSqlBuilder(List<DBCodeItem> items)
        {
            _sqlBuilder = new StringBuilder();
            long startCodeId = GetStartCodeId();
             var codeTracks=new List<CodeTrack>();
            items.ForEach(p =>
            {
                CodeTrack codeTrack = GetCodeTrackByCode(p.Code);
                long codeId = codeTrack?.SysCodeId ?? startCodeId;

                //生成Sql
                GeneratorItemSql(codeId, p);
                
                //插入记录、更新Id
                if (codeTrack == null)
                {
                    var addCodeTrack = p.Adapt<CodeTrack>();
                    addCodeTrack.SysCodeId = codeId;
                    addCodeTrack.UpdateTime=DateTime.Now;
                    codeTracks.Add(addCodeTrack);
                    startCodeId += Interval;
                }
            });

            InsertCodeTracks(codeTracks);
            return _sqlBuilder;
        }

        private void GeneratorItemSql(long codeId, DBCodeItem item)
        {
            bool hasThreeItem = item.KeyValues.Contains("*");
            if (hasThreeItem)
            {
                _sqlBuilder.Append($"delete from SysCode where pid in(select id from SysCode where pid={codeId});{Environment.NewLine}");
            }
            _sqlBuilder.Append($"delete from SysCode where pid={codeId};{Environment.NewLine}");
            _sqlBuilder.Append($"delete from SysCode where id={codeId};{Environment.NewLine}");
            _sqlBuilder.Append($"# {item.Name} {item.Code} {item.KeyValues}{Environment.NewLine}");
            _sqlBuilder.Append(
                $"insert into SysCode(id, code, name, ord, pid) value({codeId}, '{item.Code}', '{item.Name}', 0 , null);{Environment.NewLine}");
            var subItems = item.KeyValues.Split(".");
            int orderNum = 10;
            try
            {
                foreach (string subItem in subItems)
                {
                    var itemSplit = subItem.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    string code = itemSplit[0].Replace(" ", "");
                    string name = itemSplit[1].Replace(" ", "");

                    if (hasThreeItem)
                    {
                        var subItemSplit = name.Split("*", StringSplitOptions.RemoveEmptyEntries);
                        name = subItemSplit[0];
                        _sqlBuilder.Append(
                            $"insert into SysCode(id, code, name, ord, pid) value({codeId + orderNum}, '{code}', '{name}', {orderNum} , {codeId});{Environment.NewLine}");

                        var threeItems = subItemSplit[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
                        int threeOrder = 10;
                        foreach (var threeItem in threeItems)
                        {
                            var threeItemSplit = threeItem.Split("&", StringSplitOptions.RemoveEmptyEntries);
                            _sqlBuilder.Append(
                                $"insert into SysCode(id, code, name, ord, pid) value({(codeId + orderNum) * 100 + threeOrder}, '{threeItemSplit[0]}', '{threeItemSplit[1]}', {threeOrder} , {(codeId + orderNum)});{Environment.NewLine}");
                            threeOrder += 10;
                        }
                    }
                    else
                    {
                        _sqlBuilder.Append(
                            $"insert into SysCode(id, code, name, ord, pid) value({codeId + orderNum}, '{code}', '{name}', {orderNum} , {codeId});{Environment.NewLine}");
                    }
                    orderNum += 10;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //写入日志
                SerilogHelper.Instance.Error(e, "code:{code}, name:{name}, items:{items}", item.Code, item.Name,
                    item.KeyValues);
            }


            UpdateNewcityCodeId(item, codeId);
            _sqlBuilder.Append($"{Environment.NewLine}");
        }

        public void UpdateSqliteCodeKeyValues()
        {
            using var context = new SettingContext();
            var codeTrack = context.CodeTrack.ToList();
            using var context96 = new ADJUSTDBContext();
            codeTrack.ForEach(t =>
            {
                var fieldSetting = context96.HousingFieldSettings1
                    .FirstOrDefault(p =>
                        p.Code == t.Code
                        && !(p.Selector == null || p.Selector.Equals(String.Empty))
                        && p.Selector != t.KeyValues
                    );
                if (fieldSetting != null)
                {
                    t.KeyValues = fieldSetting.Selector;
                    t.UpdateTime=DateTime.Now;
                    context.CodeTrack.Update(t);
                    context.SaveChanges();
                }
            });
        }

        private void UpdateNewcityCodeId(DBCodeItem item, long codeId)
        {
            try
            {
                using var context = new ADJUSTDBContext();
                var record = context.HousingFieldSettings1
                    .FirstOrDefault(t => t.Code == item.Code
                                         && t.Selector == item.KeyValues
                                         && t.CodeId == null);
                if (record != null)
                {
                    record.CodeId = codeId.ToString();
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SerilogHelper.Instance.Error(e, "code:{code}, name:{name}, items:{items}", item.Code, item.Name,
                    item.KeyValues);
            }
            
        }

        private List<DBCodeItem> ReadCodeItems()
        {
            using var context = new ADJUSTDBContext();
            return context.HousingFieldSettings1
                .Where(p => !(p.Code == null || p.Code.Equals(String.Empty))
                            && !(p.Selector == null || p.Selector.Equals(String.Empty)))
                .OrderBy(t=>t.CodeId)
                .ThenBy(t => t.Date)
                .ThenBy(t=>t.Code)
                .Select(t => new DBCodeItem
                {
                    Code = t.Code,
                    Name = t.Fielddesc,
                    KeyValues = t.Selector
                })
                .ToList();
        }
    }
}