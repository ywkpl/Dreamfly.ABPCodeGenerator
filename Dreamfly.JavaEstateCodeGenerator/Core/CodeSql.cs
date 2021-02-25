using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class CodeSql
    {
        private readonly Project _project;
        public CodeSql(Project project)
        {
            this._project = project;
        }

        public void GeneratorFile()
        {
            string sqlString = GeneratorSqlString();
            Save(sqlString);
        }

        private void Save(string sqlString)
        {
            String filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\SysCode.sql");
            File.WriteAllText(filePath, sqlString, Encoding.UTF8);
        }

        private String GeneratorSqlString()
        {
            var items = ReadCodeItems();
            return GeneratorSqlBuilder(items).ToString();
        }

        private const int StartId = 20000;
        private const int Interval = 1000;
        private StringBuilder _sqlBuilder;
        
        private StringBuilder GeneratorSqlBuilder(List<DBCodeItem> items)
        {
            _sqlBuilder = new StringBuilder();
            int startId = StartId;
            int endId = startId + items.Count * Interval;
            while (startId<=endId)
            {
                var item = items.FirstOrDefault(t => t.CodeId == startId.ToString());
                bool codeNotExists = item == null;
                if (codeNotExists)
                {
                    item = items.First();
                }

                GeneratorItemSql(startId, item);
                items.Remove(item);

                //更新CodeId
                if (codeNotExists)
                {
                    item.CodeId = startId.ToString();
                    UpdateCodeId(item);
                }
                startId += Interval;
            }
            return _sqlBuilder;
        }

        private void GeneratorItemSql(int codeId, DBCodeItem item)
        {
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
                    _sqlBuilder.Append(
                        $"insert into SysCode(id, code, name, ord, pid) value({codeId + orderNum}, '{code}', '{name}', {orderNum} , {codeId});{Environment.NewLine}");
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

            _sqlBuilder.Append($"{Environment.NewLine}");
        }

        private void UpdateCodeId(DBCodeItem item)
        {
            try
            {
                using var context = new ADJUSTDBContext();
                var record = context.HousingFieldSettings1
                    .SingleOrDefault(t => t.Code == item.Code && t.Selector == item.KeyValues);
                if (record != null)
                {
                    record.CodeId = item.CodeId;
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
                .OrderBy(t => t.CodeId)
                .ThenBy(t=>t.Date)
                .Select(t => new DBCodeItem
                {
                    Code = t.Code,
                    Name = t.Fielddesc,
                    KeyValues = t.Selector,
                    CodeId = t.CodeId
                })
                .ToList();
        }
    }
}