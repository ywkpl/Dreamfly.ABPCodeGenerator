using System.Collections.Generic;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public interface IEntityPersistence
    {
        void Save(EntityDto entity);
        EntityDto Get(string entityName);
        EntityDto Get(int id);
        void DeleteItem(int itemId);
        void DeleteItems(List<int> itemIds);
        
        EntityDto Add(EntityDto dto);
        EntityDto Update(EntityDto dto);
    }
}