using System.Threading.Tasks;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public interface ITemplateEngine
    {
        Task<string> Render(Entity entity, Template template);
    }
}