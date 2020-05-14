/*
* 作者: ywk
* 时间: 2020-05-14 15:09
* 邮箱: ywkpl@hotmail.com
* 描述: 
*/
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Newcity.ReEstate.Test.Dto;
using Abp.Application.Services;
using System;
using System.Linq;
using Abp.Authorization;

namespace Newcity.ReEstate.Test
{
    //权限未加
	public class EntityAppService :
        AsyncCrudAppServiceBase<Entity, EntityDto, int, GetAllEntityInput, CreateEntityInput, UpdateEntityInput, GetEntityInput, DeleteEntityInput>,
        IEntityAppService
    {
        public EntityAppService(IRepository<Entity> repository) : base(repository)
        {
        }

        protected override IQueryable<Entity> CreateFilteredQuery(GetAllEntityInput input)
        {
            return base.CreateFilteredQuery(input);
                    //.WhereIf(input.CompanyId.HasValue, t => t.CompanyId == input.CompanyId)
        }
    }
}