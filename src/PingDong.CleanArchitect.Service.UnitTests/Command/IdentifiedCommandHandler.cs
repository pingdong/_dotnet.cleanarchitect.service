using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Xunit;

namespace PingDong.CleanArchitect.Service.UnitTests
{
    public class IdentifiedCommandHandlerTests
    {
        
        [Fact]
        public void IdentifiedCommandHandler_ThrowException_IfNullProvided()
        {
            var requestManager = new Mock<IRequestManager<Guid>>();
            var mediator = new Mock<IMediator>();

            Assert.Throws<ArgumentNullException>(() => new IdentifiedCommandHandler<Guid, string, IdentifiedCommand<Guid, string, TestRequest>>(mediator.Object, null));
            Assert.Throws<ArgumentNullException>(() => new IdentifiedCommandHandler<Guid, string, IdentifiedCommand<Guid, string, TestRequest>>(null, requestManager.Object));
        }

        [Fact]
        public async void IdentifiedCommandHandler_ReturnNull_IfDuplication()
        {
            var requestManager = new Mock<IRequestManager<Guid>>();
            requestManager.Setup(m => m.CheckExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            
            var mediator = new Mock<IMediator>();

            var handler = new IdentifiedCommandHandler<Guid, string, TestRequest>(mediator.Object, requestManager.Object);

            var command = new IdentifiedCommand<Guid, string, TestRequest>(Guid.NewGuid(), new TestRequest());

            var result = await handler.Handle(command); 
            Assert.Null(result);
        }
        
        [Fact]
        public async void IdentifiedCommandHandler_Process_IfNew()
        {
            var requestManager = new Mock<IRequestManager<Guid>>();
            requestManager.Setup(m => m.CheckExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            requestManager.Setup(m => m.CreateRequestRecordAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync("Test");
            
            var handler = new IdentifiedCommandHandler<Guid, string, TestRequest>(mediator.Object, requestManager.Object);

            var command = new IdentifiedCommand<Guid, string, TestRequest>(Guid.NewGuid(), new TestRequest());

            var result = await handler.Handle(command);
            Assert.Equal("Test", result);
        }
    }
}
