using MediatR;
using System;
using Xunit;

namespace PingDong.CleanArchitect.Service.UnitTests
{
    public class IdentifiedCommandTests
    {
        [Fact]
        public void IdentifiedCommand_ThrowException_IfValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new IdentifiedCommand<Guid, string, TestRequest>(Guid.Empty, new TestRequest()));
            Assert.Throws<ArgumentNullException>(() => new IdentifiedCommand<Guid, string, TestRequest>(Guid.NewGuid(), null));
        }

        [Fact]
        public void IdentifiedCommnd_PropertyReturn_ValueAssigned()
        {
            var request = new TestRequest { Name = "Test" };
            var command = new IdentifiedCommand<Guid, string, TestRequest>(Guid.NewGuid(), request);

            Assert.Equal("Test", command.Command.Name);
        }
    }

    internal class TestRequest : IRequest<string>
    {
        public string Name { get; set; }
    }
}
