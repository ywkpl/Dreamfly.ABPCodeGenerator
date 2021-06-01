using System.Collections.Generic;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public interface IEntityPersistence
    {
        void Save(EntityDto entity);
        EntityDto Get(string entityName);
        EntityDto Get(int id);
        EntityDto Update(EntityDto entity);
        void DeleteItem(int itemId);
        void DeleteItems(List<int> itemIds);
    }
}