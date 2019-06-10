using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MediatR;

[assembly: InternalsVisibleTo("PingDong.CleanArchitect.Service.UnitTests")]
namespace PingDong.CleanArchitect.Service
{
    internal class IdentifiedCommand<TId, TResponse, TCommand> : IRequest<TResponse> where TCommand : IRequest<TResponse>
    {
        public TCommand Command { get; }

        public TId Id { get; }

        public IdentifiedCommand(TId id, TCommand command)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default))
                throw new ArgumentNullException(nameof(id));
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            Command = command;
            Id = id;
        }
    }
}
