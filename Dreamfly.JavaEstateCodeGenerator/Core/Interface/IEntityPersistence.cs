using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public interface IEntityPersistence
    {
        void Save(EntityDto entity);
        EntityDto Get(string entityName);
    }
}