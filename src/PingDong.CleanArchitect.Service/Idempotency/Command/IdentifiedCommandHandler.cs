using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service
{
    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    /// a requestid sent by client is used to detect duplicate requests.
    /// 
    /// https://github.com/aspnet/DependencyInjection/issues/531
    /// https://github.com/aspnet/Home/issues/2341
    /// </summary>
    /// <typeparam name="TCommand">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="TResponse">Return value of the inner command handler</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class IdentifiedCommandHandler<TId, TResponse, TCommand> : IRequestHandler<IdentifiedCommand<TId, TResponse, TCommand>, TResponse>
		                                                            where TCommand : IRequest<TResponse>
	{
		private readonly IMediator _mediator;
		private readonly IRequestManager<TId> _requestManager;

		public IdentifiedCommandHandler(IMediator mediator, IRequestManager<TId> requestManager)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
		}

		/// <summary>
		/// Creates the result value to return if a previous request was found
		/// </summary>
		/// <returns></returns>
		protected virtual TResponse CreateResultForDuplicateRequest()
		{
			return default;
		}

		/// <summary>
		/// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
		/// just enqueue the original inner command.
		/// </summary>
		/// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Return value of inner command or default value if request same ID was found</returns>
		public async Task<TResponse> Handle(IdentifiedCommand<TId, TResponse, TCommand> message, CancellationToken cancellationToken = default)
		{
			var exists = await _requestManager.CheckExistsAsync(message.Id).ConfigureAwait(false);
			if (exists)
				return CreateResultForDuplicateRequest();

			await _requestManager.CreateRequestRecordAsync(message.Id);

			// Send the embedded business command to mediator so it runs its related CommandHandler 
			return await _mediator.Send(message.Command, cancellationToken);
		}
	}
}