using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Interface
{
    public interface ITemplateEngine
    {
        Task<string> Render(Entity entity);
    }
}