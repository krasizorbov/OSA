using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(OSA.Web.Areas.Identity.IdentityHostingStartup))]

namespace OSA.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
