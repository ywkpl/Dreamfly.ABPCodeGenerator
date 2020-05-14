/*
* 作者: ywk
* 时间: 2020-05-14 15:13
* 邮箱: ywkpl@hotmail.com
* 描述: 
*/
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Newcity.ReEstate.Test.Dto
{
    [AutoMapFrom(typeof(Entity)]
    public class EntityDto : IEntityDto
    {
/// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}