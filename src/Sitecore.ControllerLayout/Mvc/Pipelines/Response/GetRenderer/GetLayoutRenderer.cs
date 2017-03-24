namespace Sitecore.ControllerLayout.Mvc.Pipelines.Response.GetRenderer
{
    using System;
    using Data;
    using Data.Managers;
    using Extensions;
    using Sitecore.Mvc.Configuration;
    using Sitecore.Mvc.Extensions;
    using Sitecore.Mvc.Pipelines.Response.GetRenderer;
    using Sitecore.Mvc.Presentation;

    public class GetLayoutRenderer : GetRendererProcessor
    {
        private readonly ID _templateId;

        public GetLayoutRenderer(string templateId)
        {
            _templateId = ID.Parse(templateId);
        }

        protected virtual Tuple<string, string> GetControllerAndAction(Rendering rendering, GetRendererArgs args)
        {
            var controllerAndAction = GetFromProperties(rendering) ?? GetFromRenderingItem(rendering, args);
            if (controllerAndAction == null)
            {
                return null;
            }

            return MvcSettings.ControllerLocator.GetControllerAndAction(controllerAndAction.Item1, controllerAndAction.Item2);
        }

        protected virtual Tuple<string, string> GetFromProperties(Rendering rendering)
        {
            var controller = rendering["Controller"];
            var action = rendering["Controller Action"];
            if (controller.IsWhiteSpaceOrNull())
            {
                return null;
            }

            return new Tuple<string, string>(controller, action);
        }

        protected virtual Tuple<string, string> GetFromRenderingItem(Rendering rendering, GetRendererArgs args)
        {
            ID layoutId;
            if (!ID.TryParse(rendering.LayoutId, out layoutId))
            {
                return null;
            }

            var item = rendering.RenderingItem.Database.GetItem(layoutId);
            if (item?.Template == null)
            {
                return null;
            }

            if (!item.Template.DescendsFrom(_templateId))
            {
                return null;
            }

            var controller = item["Controller"];
            var action = item["Action"];

            if (controller.IsWhiteSpaceOrNull())
            {
                return null;
            }

            return new Tuple<string, string>(controller, action);
        }

        protected virtual Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
        {
            var controllerAndAction = GetControllerAndAction(rendering, args);
            if (controllerAndAction == null)
            {
                return null;
            }

            return new ControllerRenderer()
            {
                ControllerName = controllerAndAction.Item1,
                ActionName = controllerAndAction.Item2
            };
        }

        public override void Process(GetRendererArgs args)
        {
            if (args.Result != null)
            {
                return;
            }

            if (args.Rendering.RenderingType != "Layout")
            {
                return;
            }

            args.Result = this.GetRenderer(args.Rendering, args);
        }
    }
}