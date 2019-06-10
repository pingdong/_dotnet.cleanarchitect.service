using Microsoft.Extensions.DependencyInjection;

namespace PingDong.CleanArchitect.Service
{
    public class ServiceRegistrar
    {
        public virtual void Register(IServiceCollection services)
        {
            // Idempotent: Register RequestManager
            services.AddScoped(typeof(IRequestManager<>), typeof(RequestManager<>));
        }
    }
}
