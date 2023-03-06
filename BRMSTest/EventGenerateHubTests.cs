using BRMSystem.EventGenerator;
using DataAccessLayer.Models;
using EventGenerator;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMSTest
{
    [TestClass]
    public class EventGenerateHubTests
    {
        private Mock<IHubClients> _mockHubClients;
        private Mock<IHubContext<EventHub>> _mockHubContext;
        private Alert _alert;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHubClients = new Mock<IHubClients>();
            _mockHubContext = new Mock<IHubContext<EventHub>>();
            _mockHubContext.Setup(x => x.Clients).Returns(_mockHubClients.Object);

            _alert = new Alert { BorrowerId = 2, Message = "This is a test alert." };
        }

        [TestMethod]
        public void GenerateEvents_AlertIsNull_ShouldNotSendAlert()
        {
            // Arrange
            var eventGenerateHub = new EventGenerateHub(_mockHubContext.Object, null);

            // Act
            eventGenerateHub.GenerateEvents(null);

            // Assert
            _mockHubClients.Verify(x => x.All.SendAsync("ReceiveEvent", _alert, It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public void GenerateEvents_AlertIsNotNull_ShouldSendAlert()
        {
            // Arrange
            var eventGenerateHub = new EventGenerateHub(_mockHubContext.Object, _alert);

            // Act
            eventGenerateHub.GenerateEvents(null);

            // Assert
            _mockHubClients.Verify(x => x.All.SendAsync("ReceiveEvent", _alert, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
