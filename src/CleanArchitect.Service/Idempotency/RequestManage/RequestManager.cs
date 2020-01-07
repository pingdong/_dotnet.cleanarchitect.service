using PingDong.CleanArchitect.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service
{
    public class RequestManager<TId> : IRequestManager<TId>
    {
        private readonly IClientRequestRepository<TId> _repository;

        public RequestManager(IClientRequestRepository<TId> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task EnsureNotExistsAsync(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default))
                throw new ArgumentNullException(nameof(id));

            var request = await _repository.FindByIdAsync(id);

            if (request != default)
                throw new RequestDuplicatedException(id.ToString());
        }

        public async Task CreateRequestRecordAsync(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default))
                throw new ArgumentNullException(nameof(id));

            await EnsureNotExistsAsync(id);

            var request = new ClientRequest<TId>(id, typeof(TId).Name, DateTime.UtcNow);

            await _repository.AddAsync(request);

            await _repository.SaveChangesAsync();
        }
    }
}
