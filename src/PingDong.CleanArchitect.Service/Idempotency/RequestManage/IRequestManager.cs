using System;
using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service.Idempotency
{
    internal interface IRequestManager
    {
        Task<bool> CheckExistsAsync(Guid id);

        Task CreateRequestRecordAsync<T>(Guid id);
    }
}
