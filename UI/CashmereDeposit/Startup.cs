
// Type: CashmereDeposit.Startup

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Web.Http;
using Owin;

namespace CashmereDeposit
{
  public class Startup
  {
    public void Configuration(IAppBuilder appBuilder)
    {
      HttpConfiguration configuration = new HttpConfiguration();
      configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", (object) new
      {
        id = RouteParameter.Optional
      });
      appBuilder.Use(configuration);
    }
  }
}
