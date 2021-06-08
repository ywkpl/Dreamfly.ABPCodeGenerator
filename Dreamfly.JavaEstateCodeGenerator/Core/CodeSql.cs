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
        private List<Tuple<long, string>> _sqlTuples;

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
            GeneratorSqlBuilder(items);
            var sqlList = _sqlTuples
                .OrderBy(t => t.Item1)
                .Select(t => t.Item2);
            return String.Join("", sqlList);
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

        private void GeneratorSqlBuilder(List<DBCodeItem> items)
        {
            _sqlTuples = new List<Tuple<long, string>>();
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
        }



        private void GeneratorItemSql(long codeId, DBCodeItem item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            bool hasThreeItem = item.KeyValues.Contains("*");
            if (hasThreeItem)
            {
                sqlBuilder.Append(
                    $"delete from SysCode where pid in(select id from SysCode where pid={codeId});{Environment.NewLine}");
            }

            sqlBuilder.Append($"delete from SysCode where pid={codeId};{Environment.NewLine}");
            sqlBuilder.Append($"delete from SysCode where id={codeId};{Environment.NewLine}");
            sqlBuilder.Append($"# {item.Name} {item.Code} {item.KeyValues}{Environment.NewLine}");
            sqlBuilder.Append(
                $"insert into SysCode(id, code, name, ord, pid, rank) value({codeId}, '{item.Code}', '{item.Name}', 0 , null, 1);{Environment.NewLine}");
            var subItems = item.KeyValues.Split(".");
            int orderNum = 10;
            try
            {
                if (subItems.Length > 0)
                {
                    StringBuilder secondSql =
                        new StringBuilder(
                            $"{Environment.NewLine}insert into SysCode(id, code, name, ord, pid, rank) values");

                    StringBuilder thirdSqlAll = new StringBuilder();
                    foreach (string subItem in subItems)
                    {
                        var itemSplit = subItem.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        string code = itemSplit[0].Replace(" ", "");
                        string name = itemSplit[1].Replace(" ", "");

                        if (hasThreeItem)
                        {
                            var subItemSplit = name.Split("*", StringSplitOptions.RemoveEmptyEntries);
                            name = subItemSplit[0];
                            secondSql.Append(
                                $"({Environment.NewLine}{codeId + orderNum}, '{code}', '{name}', {orderNum} , {codeId}, 2),");

                            var threeItems = subItemSplit[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
                            int threeOrder = 10;
                            StringBuilder thirdSql = new StringBuilder(
                                $"{Environment.NewLine}insert into SysCode(id, code, name, ord, pid, rank) values");
                            foreach (var threeItem in threeItems)
                            {
                                var threeItemSplit = threeItem.Split("&", StringSplitOptions.RemoveEmptyEntries);
                                thirdSql.Append(
                                    $"{Environment.NewLine}({(codeId + orderNum) * 100 + threeOrder}, '{threeItemSplit[0]}', '{threeItemSplit[1]}', {threeOrder} , {(codeId + orderNum)}, 3),");
                                threeOrder += 10;
                            }

                            thirdSql.Remove(thirdSql.Length - 1, 1).Append(";");
                            thirdSqlAll.Append(thirdSql);
                        }
                        else
                        {
                            secondSql.Append(
                                $"({Environment.NewLine}{codeId + orderNum}, '{code}', '{name}', {orderNum} , {codeId}, 2),");
                        }

                        orderNum += 10;
                    }

                    secondSql.Remove(secondSql.Length - 1, 1).Append(";");
                    sqlBuilder.Append(secondSql);
                    if(thirdSqlAll.Length>0)
                        sqlBuilder.Append(thirdSqlAll);
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
            sqlBuilder.Append($"{Environment.NewLine}");
            
            _sqlTuples.Add(new Tuple<long, string>(codeId, sqlBuilder.ToString()));
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