using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public interface IEntityPersistence
    {
        void Save(Entity entity);
        Entity Get(string entityName);
    }
}