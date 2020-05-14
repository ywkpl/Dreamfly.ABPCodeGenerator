/*
* 作者: ywk
* 时间: 2020-05-14 15:21
* 邮箱: ywkpl@hotmail.com
* 描述: 
*/
using System;
using Abp.Application.Services.Dto;


namespace Newcity.ReEstate.Test.Dto
{
    public class GetAllEntityInput: PagedAndSortedResultRequestDto
    {
/// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
/// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
    }
}