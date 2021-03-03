using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public abstract class InsertCodeToFileBase
    {
        protected EntityDto entity;

        protected InsertCodeToFileBase(EntityDto entity)
        {
            this.entity = entity;
        }

        public abstract void Insert();
        public abstract void Remove();
    }
}