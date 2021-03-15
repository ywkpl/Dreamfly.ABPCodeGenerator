using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class EntityDto
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public bool HasIHasCompany { get; set; }
        public bool HasIHasTenant { get; set; }
        public Project Project { get; set; }
        public bool IsSync { get; set; }
        public List<EntityItemDto> EntityItems { get; set; }
    }

    public class EntityItemDto
    {
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
        /// <summary>
        /// 关联类型[OneToOne, OneToMany, ManyToOne, ManyToMany]
        /// </summary>
        public string RelateType { get; set; }
        /// <summary>
        /// 维护类型/级联类型[CascadeType]
        /// </summary>
        public string CascadeType { get; set; }
        /// <summary>
        /// 关联实体
        /// </summary>
        public string RelateEntity { get; set; }

        /// <summary>
        /// 关联方向：Join / MappedBy
        /// </summary>
        public string RelateDirection { get; set; }

        /// <summary>
        /// Join关联名称，看能否自动
        /// </summary>
        public string JoinName { get; set; }

        /// <summary>
        /// 外键名称
        /// </summary>
        public string ForeignKeyName { get; set; }
    }
}