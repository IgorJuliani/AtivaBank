using AtivaBank.Domain.Entities;

namespace AtivaBank.Domain.Repositories
{
    public interface IAccountRepository
    {
        decimal GetBalance(string id);
        Account GetAccount(string id);
        decimal WithdrawValue(string id, decimal value);
        void CreateAccount(Account newAccount);
        bool TransferValue(decimal value, Account accountSender, Account accountReceiver);
        decimal DepositValue(string id, decimal value);
    }
}
