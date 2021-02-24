using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public void GeneraterFile(ImportExcelDto dto)
        {
            string sqlString = GeneraterSqlString(dto);
            Save(sqlString);
        }

        private void Save(string sqlString)
        {
            String filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\SysCode.sql");
            File.WriteAllText(filePath, sqlString, Encoding.UTF8);
        }

        private String GeneraterSqlString(ImportExcelDto dto)
        {
            var items = ReadCodeItems();
            var sqlBuilder = GeneratorSqlBuilder(items);
            return sqlBuilder.ToString();
        }

        private StringBuilder GeneratorSqlBuilder(List<DBCodeItem> items)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            int codeId = 21000;
            items.ForEach(t =>
            {
                sqlBuilder.Append($"delete from SysCode where pid={codeId};{Environment.NewLine}");
                sqlBuilder.Append($"delete from SysCode where id={codeId};{Environment.NewLine}");
                sqlBuilder.Append($"# {t.Name} {t.Code} {t.KeyValues}{Environment.NewLine}");
                sqlBuilder.Append(
                    $"insert into SysCode(id, code, name, ord, pid) value({codeId}, '{t.Code}', '{t.Name}', 0 , null);{Environment.NewLine}");
                var subItems = t.KeyValues.Split(".");
                int orderNum = 10;
                try
                {
                    foreach (string subItem in subItems)
                    {
                        var item = subItem.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        string code = item[0].Replace(" ", "");
                        string name = item[1].Replace(" ", "");
                        sqlBuilder.Append(
                            $"insert into SysCode(id, code, name, ord, pid) value({codeId + orderNum}, '{code}', '{name}', {orderNum} , {codeId});{Environment.NewLine}");
                        orderNum += 10;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //写入日志
                }
                

                sqlBuilder.Append($"{Environment.NewLine}");

                codeId += 1000;
            });
            return sqlBuilder;
        }

        private List<DBCodeItem> ReadCodeItems()
        {
            using var context = new ADJUSTDBContext();
            return context.HousingFieldSettings1s
                .Where(p => !(p.Code == null || p.Code.Equals(String.Empty))
                            && !(p.Selector == null || p.Selector.Equals(String.Empty)))
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