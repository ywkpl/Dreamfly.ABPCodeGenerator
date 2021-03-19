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
            var findEntity = GetEntityById(dto.Name);
            var entity = ToEntity(dto);
            using var context=new SettingContext();
            if (findEntity != null)
            {
                context.Remove(findEntity);
                context.SaveChanges();
                entity.Id = findEntity.Id;
            }
            context.Add(entity);
            context.SaveChanges();
        }

        private Entity GetEntityById(string entityName)
        {
            using var context = new SettingContext();
            return context.Entity
                .Include(t => t.EntityItems.OrderBy(g=>g.Order))
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
            var entity = GetEntityById(entityName);
            return ToEntityDto(entity);
        }
    }
}