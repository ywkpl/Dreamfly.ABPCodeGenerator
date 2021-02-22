using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl
{
    public class EntityPersistence : IEntityPersistence
    {
        private readonly List<Entity> _entities = JsonDataHelper.ReadEntities();

        public void Save(Entity entity)
        {
            var getEntity = Get(entity.Name);
            if (getEntity != null)
            {
                _entities.Remove(getEntity);
            }

//            if (_entities.Any(t => t.Name == entity.Name))
//            {
//                int index = _entities.FindIndex(t => t.Name == entity.Name);
//                _entities[index] = entity;
//            }
//            else
//            {
            _entities.Add(entity);
//            }

            JsonDataHelper.SaveEntities(_entities);
        }

        public Entity Get(string entityName)
        {
            return _entities.FirstOrDefault(t => t.Name == entityName);
        }
    }
}