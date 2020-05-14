/*
* 作者: ywk
* 时间: 2020-05-14 11:44
* 邮箱: ywkpl@hotmail.com
* 描述: 
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Newcity.ReEstate.Test
{
    public class Entity : FullAuditedEntity
    {
/// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
/// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
/// <summary>
        /// 总额
        /// </summary>
        public decimal Total { get; set; }
    }
}