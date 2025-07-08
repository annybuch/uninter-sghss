using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Loop.SGHSS.Tests.Factories
{
    public class LoopTestFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Development"); 

            return base.CreateHost(builder);
        }
    }
}
