using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public interface IProjectBuilder
    {
        Task Build(Entity entity);

        Task Remove(Entity entity);
    }
}