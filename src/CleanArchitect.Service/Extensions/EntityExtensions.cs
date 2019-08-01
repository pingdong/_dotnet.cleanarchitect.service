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
        /// <param name="tracker"></param>
        /// <returns></returns>
        public static T Preprocess<T>(this T entity, ITracker tracker) where T : ITracker
        {
            entity.CorrelationId = tracker.CorrelationId;
            entity.TenantId = tracker.TenantId;

            return entity;
        }
    }
}
