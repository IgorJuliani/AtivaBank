using AtivaBank.Application.Servicos;
using AtivaBank.Controllers;
using AtivaBank.Domain.Dto;
using AtivaBank.Domain.Entities;
using AtivaBank.Domain.Repositories;
using AtivaBank.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AtivaBank.Test
{
    public class MovementControllerTest
    {

        private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<IMovementRepository> _movementRepoMock;
        private readonly Mock<ILogger<BankService>> _loggerMock;

        private readonly IBankService _bankService;
        private readonly MovementController _controller;

        public MovementControllerTest()
        {
            _accountRepoMock = new Mock<IAccountRepository>();
            _movementRepoMock = new Mock<IMovementRepository>();
            _loggerMock = new Mock<ILogger<BankService>>();

            _bankService = new BankService(_accountRepoMock.Object, _movementRepoMock.Object, _loggerMock.Object);

            _controller = new MovementController(_bankService);
        }

        [Fact]
        public void DepositValue_Success()
        {
            //Arrange
            var id = "13622871725";
            _accountRepoMock.Setup(x => x.DepositValue(id, 100))
                .Returns(default(decimal));

            _accountRepoMock.Setup(x => x.GetAccount(id))
                .Returns(new Account() { Id = "teste" });

            _movementRepoMock.Setup(x => x.RegisterMovement(It.IsAny<Movements>()));

            //Act
            var resultado = _controller.DepositValue(new AlterBalanceDto(100), id);

            //Assert
            Assert.True(((NoContentResult)resultado).StatusCode == 204);
        }

        [Fact]
        public void WithdrawValue_Success()
        {
            //Arrange
            var id = "13622871725";
            _accountRepoMock.Setup(x => x.WithdrawValue(id, 100))
                .Returns(default(decimal));

            _accountRepoMock.Setup(x => x.GetAccount(id))
                .Returns(new Account() { Id = "teste", Balance = 1000000 });

            _movementRepoMock.Setup(x => x.RegisterMovement(It.IsAny<Movements>()));

            //Act
            var resultado = _controller.WithdrawValue(new AlterBalanceDto(100), id);

            //Assert
            Assert.True(((NoContentResult)resultado).StatusCode == 204);
        }

        [Fact]
        public void TransferValue_Success()
        {
            //Arrange
            var id1 = "13622871725";
            var id2 = "12323343234";

            _accountRepoMock.Setup(x => x.TransferValue(It.IsAny<decimal>(), It.IsAny<Account>(), It.IsAny<Account>()))
                .Returns(true);

            _accountRepoMock.Setup(x => x.GetAccount(It.IsAny<string>()))
                .Returns(new Account() { Id = "teste" });

            _accountRepoMock.Setup(x => x.GetBalance(It.IsAny<string>()))
                .Returns(default(decimal));

            _movementRepoMock.Setup(x => x.RegisterMovement(It.IsAny<Movements>()));

            //Act
            var resultado = _controller.TransferValue(new TransferDto(id1, id2, 10));

            //Assert
            Assert.True(((NoContentResult)resultado).StatusCode == 204);
        }
    }
}