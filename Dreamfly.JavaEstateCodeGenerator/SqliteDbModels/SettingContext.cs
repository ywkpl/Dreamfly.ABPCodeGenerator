using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dreamfly.JavaEstateCodeGenerator.SqliteDbModels
{
    public class SettingContext:DbContext
    {
        public DbSet<Entity> Entity { get; set; }
        public DbSet<EntityItem> EntityItem { get; set; }
        public DbSet<CodeTrack> CodeTrack { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=./data/setting.db");
    }

    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public bool HasIHasCompany { get; set; }
        public bool HasIHasTenant { get; set; }
        public bool IsSync { get; set; }
        public List<EntityItem> EntityItems { get; set; }
    }

    public class EntityItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public int? Length { get; set; }
        public int? Fraction { get; set; }
        public bool HasTime { get; set; }
        public bool IsRequired { get; set; }
        public bool InQuery { get; set; }
        public bool InResponse { get; set; }
        public bool InCreate { get; set; }
        public string Description { get; set; }
    }

    public class CodeTrack
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string KeyValues { get; set; }
        public long SysCodeId { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}