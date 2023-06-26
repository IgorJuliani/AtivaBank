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
    public class AccountControllerTest
    {

        private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<IMovementRepository> _movementRepoMock;
        private readonly Mock<ILogger<BankService>> _loggerMock;

        private readonly IBankService _bankService;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            _accountRepoMock = new Mock<IAccountRepository>();
            _movementRepoMock = new Mock<IMovementRepository>();
            _loggerMock = new Mock<ILogger<BankService>>();

            _bankService = new BankService(_accountRepoMock.Object, _movementRepoMock.Object, _loggerMock.Object);

            _controller = new AccountController(_bankService);
        }

        [Fact]
        public void GetBalance_Success()
        {
            //Arrange
            var id = "13622871725";
            _accountRepoMock.Setup(x => x.GetBalance(id))
                .Returns(default(decimal));

            _accountRepoMock.Setup(x => x.GetAccount(id))
                .Returns(new Account() { Id = "teste" });

            //Act
            var resultado = _controller.GetBalance(id);

            //Assert
            Assert.NotNull(((OkObjectResult)resultado).Value);
            Assert.True(((OkObjectResult)resultado).StatusCode == 200);
        }

        [Fact]
        public void CreateAccount_Success()
        {
            //Arrange
            var account = new Account() { Id = "teste", Balance = 0 };

            _accountRepoMock.Setup(x => x.CreateAccount(account));

            _accountRepoMock.Setup(x => x.GetAccount("teste"))
                .Returns(new Account());

            //Act
            var resultado = _controller.CreateAccount(new AccountDto("TEste", "Teste testando", 1000));

            //Assert
            Assert.NotNull(((CreatedResult)resultado).Value);
            Assert.True(((CreatedResult)resultado).StatusCode == 201);
        }
    }
}