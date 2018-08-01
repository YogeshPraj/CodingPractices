using System.Web.Http;
using WebActivatorEx;
using WebApplication1;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApplication1
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(UpdateConfig)
                .EnableSwaggerUi(c =>
                {

                });
        }

        private static void UpdateConfig(SwaggerDocsConfig c)
        {
            c.Schemes(new[] { "http", "https" });
            c.SingleApiVersion("v1", "DemoAPI").Description("Specification for Demo API");
        }
    }
}
