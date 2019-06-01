using Microsoft.Extensions.DependencyInjection;
using PingDong.CleanArchitect.Service.Idempotency;

namespace PingDong.CleanArchitect.Service
{
    public class ServiceRegistrar
    {
        public virtual void Register(IServiceCollection services)
        {
            // Idempotency: Register RequestManager
            services.AddScoped(typeof(IRequestManager), typeof(RequestManager));
        }
    }
}
