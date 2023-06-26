using AtivaBank.Controllers.Base;
using AtivaBank.Domain.Dto;
using AtivaBank.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtivaBank.Controllers
{
    [ApiController]
    [Route("v1/movement")]
    public class MovementController : DefaultController
    {

        private readonly IBankService _bankService;

        public MovementController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPatch("deposit/{id}")]
        public IActionResult DepositValue([FromBody] AlterBalanceDto alter, string id)
            => GetResponseNoContent(_bankService.DepositValue(alter.value, id));


        [HttpPatch("withdraw/{id}")]
        public IActionResult WithdrawValue([FromBody] AlterBalanceDto alter, string id)
            => GetResponseNoContent(_bankService.WithdrawValue(alter.value, id));
    

        [HttpPatch("transfer")]
        public IActionResult TransferValue([FromBody] TransferDto alter)
            => GetResponseNoContent(_bankService.TransferToAccount(alter.senderId, alter.receiverId, alter.value));
    }
}