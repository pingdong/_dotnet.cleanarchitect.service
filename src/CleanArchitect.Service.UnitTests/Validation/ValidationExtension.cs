using FluentValidation;
using FluentValidation.Results;
using PingDong.CleanArchitect.Core;
using PingDong.CleanArchitect.Core.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PingDong.CleanArchitect.Service.UnitTests
{
    public class ValidatorExtensionTests
    {
        private class TestValidator : IValidator<TestObject>
        {
            private readonly string _target;
            public TestValidator(string target)
            {
                _target = target;
            }

            public ValidationResult Validate(object instance)
            {
                throw new NotImplementedException();
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

            public ValidationResult Validate(TestObject instance)
            {
                if (instance.Name != _target)
                    return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("A", "error") });

                return new ValidationResult();
            }

            public Task<ValidationResult> ValidateAsync(TestObject instance, CancellationToken cancellation = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public CascadeMode CascadeMode { get; set; }
        }

        public class TestObject : IAggregateRoot
        {
            public string Name { get; set; }
        }

        [Fact]
        public void ValidatorExtension_Invalid()
        {
            var validators = new List<IValidator<TestObject>> { new TestValidator("abc") };
            var target = new TestObject { Name = "ABC" };

            Assert.Throws<ValidationException>(() => validators.Validate(target));
        }

        [Fact]
        public void ValidatorExtension_Valid()
        {
            var validators = new List<IValidator<TestObject>> { new TestValidator("abc") };
            var target = new TestObject { Name = "abc" };

            validators.Validate(target);
        }
    }
}
