using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dreamfly.ABPCodeGenerator.Core.Interface;
using Dreamfly.ABPCodeGenerator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class RazorTemplateEngine : ITemplateEngine
    {
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRazorViewEngine _viewEngine;

        public RazorTemplateEngine(
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IRazorViewEngine viewEngine)
        {
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _viewEngine = viewEngine;
        }

        public async Task<string> Render(Entity entity, Template template)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, template.File);
            var renderEntity = MapperRenderEntity(entity, template);

            using (var output = new StringWriter())
            {
                var viewContext = MakeViewContext(actionContext, view, renderEntity, output);
                await view.RenderAsync(viewContext);
                return output.ToString();
            }
        }

        private ViewContext MakeViewContext(ActionContext actionContext, IView view, RenderEntity renderEntity,
            StringWriter output)
        {
            return new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<RenderEntity>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = renderEntity
                },
                new TempDataDictionary(
                    actionContext.HttpContext,
                    _tempDataProvider),
                output,
                new HtmlHelperOptions()
            );
        }

        private RenderEntity MapperRenderEntity(Entity entity, Template template)
        {
            return new RenderEntity
            {
                ProjectName = entity.Project.Name,
                EntityName = entity.Name,
                ModuleName = entity.Module,
                Author = entity.Project.Author,
                Template = template,
                EntityItems = entity.EntityItems
            };
        }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] {$"Unable to find view '{viewName}'. The following locations were searched:"}.Concat(
                    searchedLocations));
            ;

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}