using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PingDong.CleanArchitect.Infrastructure;

namespace PingDong.CleanArchitect.Service
{
    internal class RequestManager<TId> : IRequestManager<TId>
    {
        private readonly IRepository<TId, ClientRequest<TId>> _repository;

        public RequestManager(IRepository<TId, ClientRequest<TId>> context)
        {
            _repository = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CheckExistsAsync(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default))
                return false;

            var request = await _repository.FindByIdAsync(id);

            return request != default;
        }

        public async Task CreateRequestRecordAsync(TId id)
        { 
            if (EqualityComparer<TId>.Default.Equals(id, default))
                throw new ArgumentNullException(nameof(id));

            var exists = await CheckExistsAsync(id);

            var request = exists 
                            ? throw new RequestDuplicatedException($"Request with {id} already exists") 
                            : new ClientRequest<TId>(typeof(TId).Name, DateTime.UtcNow);

            await _repository.AddAsync(request);

            await _repository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
