using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string TableName { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public bool HasIHasCompany { get; set; }
        public bool HasIHasTenant { get; set; }
        public bool IsSync { get; set; }
        public List<EntityItem> EntityItems { get; set; }
    }

    public class EntityItem
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string ColumnName { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        public int? Length { get; set; }
        public int? Fraction { get; set; }
        public bool HasTime { get; set; }
        public bool IsRequired { get; set; }
        public bool InQuery { get; set; }
        public bool InResponse { get; set; }
        public bool InCreate { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// 关联类型[OneToOne, OneToMany, ManyToOne, ManyToMany]
        /// </summary>
        [MaxLength(20)]
        public string RelateType { get; set; }
        /// <summary>
        /// 维护类型/级联类型[CascadeType]
        /// </summary>
        [MaxLength(20)]
        public string CascadeType { get; set; }
        /// <summary>
        /// 关联实体
        /// </summary>
        [MaxLength(50)]
        public string RelateEntity { get; set; }

        /// <summary>
        /// 关联方向：Join / MappedBy
        /// </summary>
        [MaxLength(50)]
        public string RelateDirection { get; set; }

        /// <summary>
        /// Join关联名称，看能否自动
        /// </summary>
        [MaxLength(50)]
        public string JoinName { get; set; }
        
        /// <summary>
        /// 外键名称
        /// </summary>
        [MaxLength(100)]
        public string ForeignKeyName { get; set; }
    }

    public class CodeTrack
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string KeyValues { get; set; }
        public long SysCodeId { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}