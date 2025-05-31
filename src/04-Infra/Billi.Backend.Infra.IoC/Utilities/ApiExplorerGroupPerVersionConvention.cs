using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Billi.Backend.Infra.IoC.Utilities
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var versions = controller.Attributes.OfType<ApiVersionAttribute>().ToList();

            if (versions.Count == 0)
            {
                return;
            }

            foreach (var version in versions)
            {
                controller.ApiExplorer.GroupName = "v" + version.Versions[0].ToString();
            }
        }
    }
}
