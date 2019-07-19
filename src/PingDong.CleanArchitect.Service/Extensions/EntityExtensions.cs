using PingDong.CleanArchitect.Core;

namespace PingDong.Newmoon.Places.Service.Commands
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Prepare an instance of Entity
        /// </summary>
        /// <typeparam name="TEntityId"></typeparam>
        /// <param name="entity"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static Entity<TEntityId> Preprocess<TEntityId>(this Entity<TEntityId> entity, IMetadata metadata)
        {
            entity.CorrelationId = metadata.CorrelationId;
            entity.TenantId = metadata.TenantId;

            return entity;
        }
    }
}
