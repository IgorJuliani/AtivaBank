using AtivaBank.Domain.Dto;
using AtivaBank.Domain.Entities;
using AtivaBank.Domain.Response;

namespace AtivaBank.Domain.Services
{
    public interface IBankService
    {
        Response<bool> TransferToAccount(string accountId1, string accountId2, decimal value);
        Response<decimal> GetBalance(string id);
        Response<decimal> WithdrawValue(decimal value, string id);
        Response<decimal> DepositValue(decimal value, string id);
        Response<Account> CreateAccount(AccountDto newAccount);
        Response<IEnumerable<Movements>> GetAccountStatement(StatementFilterDto filter);
    }
}
