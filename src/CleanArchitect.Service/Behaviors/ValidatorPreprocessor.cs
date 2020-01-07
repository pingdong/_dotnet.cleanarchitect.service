using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using PingDong.CleanArchitect.Core;
using PingDong.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service
{
    public class ValidatorPreprocessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IValidator<TRequest>[] _validators;
        private readonly ILogger<ValidatorPreprocessor<TRequest>> _logger;

        public ValidatorPreprocessor(IValidator<TRequest>[] validators, ILogger<ValidatorPreprocessor<TRequest>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (_validators.IsNullOrEmpty())
                return Task.CompletedTask;

            var failures = _validators.Select(v => v.Validate(request))
                                        .SelectMany(result => result.Errors)
                                        .Where(error => error != null)
                                        .ToList();

            if (failures.Any())
            {
                var tracker = request as ITracker;

                _logger.LogWarning(EventIds.ViolateDataValidation
                                    , $"Command Validation Errors for type {typeof(TRequest).Name}"
                                    , failures);

                throw new EntityException(EventIds.ViolateDataValidation
                                            , $"Command Validation Errors for type {typeof(TRequest).Name}"
                                            , new ValidationException("Validation exception", failures)
                                            , tracker
                                        );
            }

            return Task.CompletedTask;
        }
    }
}
