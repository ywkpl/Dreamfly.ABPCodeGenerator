using System;
using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Dreamfly.JavaEstateCodeGenerator.SqliteDbModels;
using EFCore.BulkExtensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl
{
    public class EntityPersistence : IEntityPersistence
    {
        public void Save(EntityDto dto)
        {
            var findEntity = GetEntityByName(dto.Name);
            var entity = ToEntity(dto);
            using var context = new SettingContext();
            if (findEntity != null)
            {
                context.Remove(findEntity);
                context.SaveChanges();
                entity.Id = findEntity.Id;
            }

            context.Add(entity);
            context.SaveChanges();
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public EntityDto Add(EntityDto dto)
        {
            var entity = ToEntity(dto);
            using var context = new SettingContext();
            context.Add(entity);
            context.SaveChanges();
            return ToEntityDto(entity);
        }

        /// <summary>
        /// 更新,删除身档一起
        /// </summary>
        /// <param name="dto"></param>
        public EntityDto Update(EntityDto dto)
        {
            if (dto.Id == null)
                throw new Exception("Id不得為空");

            //刪除字段
            RemoveEntityItems(dto);

            //更新
            var entity = ToEntity(dto);
            using var context = new SettingContext();
            context.Set<Entity>().Update(entity);
            context.SaveChanges();
            return Get(entity.Id);
        }

        /// <summary>
        /// 更新,删除身档一起
        /// </summary>
        /// <param name="dto"></param>
        public EntityDto Update(EntityDto dto, List<EntityItemDto> dropItems)
        {
            if (dto.Id == null)
                throw new Exception("Id不得為空");
            var entity = ToEntity(dto);
            using var context = new SettingContext();
            //刪除字段
            context.EntityItem.RemoveRange(
                dropItems.Select(t => t.Adapt<EntityItem>())
            );
            //更新
            context.Set<Entity>().Update(entity);
            context.SaveChanges();
            return Get(entity.Id);
        }

        public void RemoveEntityItems(EntityDto dto)
        {
            if (dto.Id != null)
            {
                //取身档差集【原数据有，新数据没有】，删除
                using var context = new SettingContext();
                var oldItems = context.EntityItem.Where(t => t.EntityId == dto.Id).ToList();
                var oldIds = oldItems.Select(t => t.Id).ToList();
                var newIds = dto.EntityItems.Where(t => t.Id.HasValue).Select(t => t.Id.Value).ToList();
                var deleteIds = oldIds.Except(newIds).ToList();
                var deleteItems = oldItems.Where(t => deleteIds.Contains(t.Id));
                context.EntityItem.RemoveRange(deleteItems);
                context.SaveChanges();
            }
        }

        public void DeleteItem(int itemId)
        {
            using var context = new SettingContext();
            var entity = context.EntityItem.SingleOrDefault(t => t.Id == itemId);
            if (entity != null)
            {
                context.EntityItem.Remove(entity);
                context.SaveChanges();
            }
        }

        public void DeleteItems(List<int> itemIds)
        {
            using var context = new SettingContext();
            var entities = context.EntityItem.Where(t => itemIds.Contains(t.Id));
            if (entities.Any())
            {
                context.EntityItem.RemoveRange(entities);
                context.SaveChanges();
            }
        }


        private Entity GetEntityById(int id)
        {
            using var context = new SettingContext();
            return context.Entity
                .Include(t => t.EntityItems.OrderBy(g => g.Order))
                .FirstOrDefault(t => t.Id == id);
        }

        private Entity GetEntityByName(string entityName)
        {
            using var context = new SettingContext();
            return context.Entity
                .Include(t => t.EntityItems.OrderBy(g => g.Order))
                .FirstOrDefault(t => t.Name == entityName);
        }

        private EntityDto ToEntityDto(Entity entity)
        {
            return entity.Adapt<EntityDto>();
        }

        private Entity ToEntity(EntityDto dto)
        {
            return dto.Adapt<Entity>();
        }

        public EntityDto Get(string entityName)
        {
            var entity = GetEntityByName(entityName);
            return ToEntityDto(entity);
        }

        public EntityDto Get(int id)
        {
            var entity = GetEntityById(id);
            return ToEntityDto(entity);
        }
    }
}