using System;
using System.Threading.Tasks;
using PingDong.CleanArchitect.Infrastructure;

namespace PingDong.CleanArchitect.Service.Idempotency
{
    internal class RequestManager : IRequestManager
    {
        private readonly IRepository<Guid, ClientRequest> _repository;

        public RequestManager(IRepository<Guid, ClientRequest> context)
        {
            _repository = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CheckExistsAsync(Guid id)
        {
            var request = await _repository.FindByIdAsync(id);

            return request != null;
        }

        public async Task CreateRequestRecordAsync<T>(Guid id)
        { 
            var exists = await CheckExistsAsync(id);

            var request = exists 
                            ? throw new RequestDuplicatedException($"Request with {id} already exists") 
                            : new ClientRequest
                                {
                                    Name = typeof(T).Name,
                                    Time = DateTime.UtcNow
                                };

            await _repository.AddAsync(request);

            await _repository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
