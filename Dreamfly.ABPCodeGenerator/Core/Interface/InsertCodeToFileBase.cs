using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Interface
{
    public abstract class InsertCodeToFileBase
    {
        protected Entity entity;

        protected InsertCodeToFileBase(Entity entity)
        {
            this.entity = entity;
        }

        public abstract void Insert();
        public abstract void Remove();
    }
}