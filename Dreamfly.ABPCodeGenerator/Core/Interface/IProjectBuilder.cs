using System.Collections.Generic;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Interface
{
    public interface IProjectBuilder
    {
        Task<string> Build(Entity entity);
        
        /// <summary>
        /// 包含至项目
        /// </summary>
        /// <returns></returns>
        void IncludeToProject();
    }
}