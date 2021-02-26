using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl
{
    public class DBExternalEntity:ExternalEntity
    {
        private readonly string _tableName;
        
        public DBExternalEntity(string tableName)
        {
            this._tableName = tableName;
        }

        protected override TableSetting ReadTableSetting()
        {
            using var dbContext = new ADJUSTDBContext();
            var query = dbContext.HousingFieldSettings1
                .Join(dbContext.HousingTableSettings,
                    d => d.TablesettingsId,
                    m => m.Id,
                    (d, m) => new {d, m})
                .Where(t => t.d.Tablename == _tableName)
                .OrderBy(t => t.d.Seq)
                .Select(t => new 
                {
                    TableName = t.d.Tablename,
                    TableDesc = t.m.Description,
                    FieldName = t.d.Fieldname,
                    FieldDesc = t.d.Fielddesc
                }).ToList();

            var tableSetting = new TableSetting { Fields = new List<TableFieldSetting>() };
            if (query.Count > 0)
            {
                tableSetting.Name = query[0].TableName;
                tableSetting.Desc = query[0].TableDesc;

                tableSetting.Fields.AddRange(query.Select(t=> new TableFieldSetting
                {
                    Name = t.FieldName,
                    Desc = t.FieldDesc
                }));
            }

            return tableSetting;
        }
    }
}