using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Billi.Backend.Infra.IoC.Utilities
{
    public class RoutePrefixConvention(string prefix) : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix = new(new RouteAttribute(prefix));

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(s => s.AttributeRouteModel != null).ToList();

                if (matchedSelectors.Count != 0)
                {
                    foreach (var selector in matchedSelectors)
                    {
                        // Prepend prefix
                        selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
                        if (selector.AttributeRouteModel?.Template != null && (selector.AttributeRouteModel?.Template?.Contains("[controller]") ?? false))
                        {
                            selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.Replace("[controller]", controller.ControllerName.ToLowerInvariant());
                        }
                    }
                }
                else
                {
                    // Controllers sem rota, define a padrão
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel = _routePrefix
                    });
                }
            }
        }
    }
}
