using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace PingDong.CleanArchitect.Service
{
    public interface IRequestManager<TId>
    {
        Task<bool> CheckExistsAsync(TId id);

        Task CreateRequestRecordAsync(TId id);
    }
}
