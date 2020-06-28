using System.Collections.Generic;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Interface
{
    public interface IProjectBuilder
    {
        Task Build(Entity entity);

        Task Remove(Entity entity);
    }
}