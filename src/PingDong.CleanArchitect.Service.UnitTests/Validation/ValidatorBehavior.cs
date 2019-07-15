using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using PingDong.CleanArchitect.Core;
using Xunit;

namespace PingDong.CleanArchitect.Service.UnitTests
{
    public class ValidatorBehaviorTests
    {
        private class TestValidator : IValidator<string>
        {
            private readonly string _target;
            public TestValidator(string target)
            {
                _target = target;
            }

            public ValidationResult Validate(object instance)
            {
                return Validate(instance.ToString());
            }

            public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public ValidationResult Validate(ValidationContext context)
            {
                throw new NotImplementedException();
            }

            public Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public IValidatorDescriptor CreateDescriptor()
            {
                throw new NotImplementedException();
            }

            public bool CanValidateInstancesOfType(Type type)
            {
                throw new NotImplementedException();
            }

            public ValidationResult Validate(string instance)
            {
                if (instance != _target)
                    return new ValidationResult(new List<ValidationFailure> {new ValidationFailure("A", "error")});

                return new ValidationResult();
            }

            public Task<ValidationResult> ValidateAsync(string instance, CancellationToken cancellation = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public CascadeMode CascadeMode { get; set; }
        }

        [Fact]
        public void ValidatorBehavior_Invalid()
        {
            var validators = new List<IValidator<string>> {new TestValidator("abc")};
            var behavior = new ValidatorBehavior<string, bool>(validators.ToArray());

            Assert.ThrowsAsync<ValidationViolatedException>(() => behavior.Handle("ABC", new CancellationToken(), null));
        }

        [Fact]
        public async void ValidatorBehavior_Valid()
        {
            var validators = new List<IValidator<string>>() {new TestValidator("abc")};
            var behavior = new ValidatorBehavior<string, bool>(validators.ToArray());

            Assert.True(await behavior.Handle("abc", new CancellationToken(), () => Task.FromResult(true)));
        }
    }
}
