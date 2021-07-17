using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Entities;

namespace OffLogs.Business.Services.Kafka
{
    public interface IKafkaProducerService
    {
        Task ProduceLogMessageAsync(string applicationJwt, LogEntity logEntity, string clientIp = null);
        
        /// <summary>
        ///     Wait until all outstanding produce requests and
        ///     delivery report callbacks are completed.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token to observe whilst waiting
        ///     the returned task to complete.
        /// </param>
        /// <remarks>
        ///     This method should typically be called prior to
        ///     destroying a producer instance to make sure all
        ///     queued and in-flight produce requests are
        ///     completed before terminating.
        /// 
        ///     A related configuration parameter is
        ///     message.timeout.ms which determines the
        ///     maximum length of time librdkafka attempts
        ///     to deliver a message before giving up and
        ///     so also affects the maximum time a call to
        ///     Flush may block.
        /// 
        ///     Where this Producer instance shares a Handle
        ///     with one or more other producer instances, the
        ///     Flush method will wait on messages produced by
        ///     the other producer instances as well.
        /// </remarks>
        /// <exception cref="T:System.OperationCanceledException">
        ///     Thrown if the operation is cancelled.
        /// </exception>
        void Flush(CancellationToken cancellationToken = default);
    }
}