using PingDong.CleanArchitect.Core;

namespace PingDong.Newmoon.Places.Service.Commands
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Prepare an instance of Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static T Preprocess<T>(this T entity, IMetadata metadata) where T : IMetadata
        {
            entity.CorrelationId = metadata.CorrelationId;
            entity.TenantId = metadata.TenantId;

            return entity;
        }
    }
}
