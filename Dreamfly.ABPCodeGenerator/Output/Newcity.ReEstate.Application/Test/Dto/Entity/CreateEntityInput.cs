/*
* 作者: ywk
* 时间: 2020-05-14 15:21
* 邮箱: ywkpl@hotmail.com
* 描述: 
*/
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Newcity.ReEstate.Test.Dto
{
    [AutoMapTo(typeof(Entity)]
    public class CreateEntityInput
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
    }
}