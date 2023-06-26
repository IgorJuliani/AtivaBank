using AtivaBank.Controllers.Base;
using AtivaBank.Domain.Dto;
using AtivaBank.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtivaBank.Controllers
{
    [ApiController]
    [Route("v1/account")]
    public class AccountController : DefaultController
    {
        private readonly IBankService _bankService;

        public AccountController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet("saldo/{id}")]
        public IActionResult GetBalance(string id)
            => GetResponse(_bankService.GetBalance(id));

        [HttpGet("account-statement")]
        public IActionResult GetStatement([FromQuery] StatementFilterDto filter)
            => GetResponse(_bankService.GetAccountStatement(filter));

        [HttpPost]
        public IActionResult CreateAccount([FromBody] AccountDto newAccount)
            => GetResponseCreated(_bankService.CreateAccount(newAccount));
    }
}