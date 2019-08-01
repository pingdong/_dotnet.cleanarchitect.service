using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PingDong.CleanArchitect.Core;
using PingDong.CleanArchitect.Infrastructure;
using Xunit;

namespace PingDong.CleanArchitect.Service.UnitTests
{
    public class RequestManagerTests
    {
        [Fact]
        public void RequestManager_ThrowException_IfRepositoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RequestManager<Guid>(null));
        }

        [Fact]
        public async void RequestManager_CheckExists_ReturnTrue_IfExists()
        {
            var mock = new Mock<IClientRequestRepository<Guid>>();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ClientRequest<Guid>(Guid.NewGuid() ,"Test", DateTime.Now));

            var requestManager = new RequestManager<Guid>(mock.Object);
            await Assert.ThrowsAsync<RequestDuplicatedException>(() => requestManager.EnsureNotExistsAsync(Guid.NewGuid()));

            mock.Verify(m => m.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void RequestManager_CheckExists_ReturnTrue_IfNotExists()
        {
            var mock = new Mock<IClientRequestRepository<Guid>>();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<ClientRequest<Guid>>(null));

            var requestManager = new RequestManager<Guid>(mock.Object);
            await requestManager.EnsureNotExistsAsync(Guid.NewGuid());

            mock.Verify(m => m.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void RequestManager_Create_IfIdIsEmpty()
        {
            var mock = new Mock<IClientRequestRepository<Guid>>();

            var requestManager = new RequestManager<Guid>(mock.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => requestManager.CreateRequestRecordAsync(Guid.Empty));
        }

        [Fact]
        public async void RequestManager_Create_IfNotExists()
        {
            var mock = new Mock<IClientRequestRepository<Guid>>();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<ClientRequest<Guid>>(null));
            mock.Setup(m => m.AddAsync(It.IsAny<ClientRequest<Guid>>())).Returns(Task.CompletedTask);

            var requestManager = new RequestManager<Guid>(mock.Object);
            await requestManager.CreateRequestRecordAsync(Guid.NewGuid());
            
            mock.Verify(m => m.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
            mock.Verify(m => m.AddAsync(It.IsAny<ClientRequest<Guid>>()), Times.Once);
        }

        [Fact]
        public async void RequestManager_Create_IfExists()
        {
            var mock = new Mock<IClientRequestRepository<Guid>>();
            mock.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ClientRequest<Guid>(Guid.NewGuid() ,"Test", DateTime.Now));
            
            var requestManager = new RequestManager<Guid>(mock.Object);
            
            await Assert.ThrowsAsync<RequestDuplicatedException>(() => requestManager.CreateRequestRecordAsync(Guid.NewGuid()));
            
            mock.Verify(m => m.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
