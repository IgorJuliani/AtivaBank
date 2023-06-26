using AtivaBank.Domain.Entities;
using AtivaBank.Domain.Exceptions;
using AtivaBank.Domain.Repositories;

namespace AtivaBank.Infra
{
    public class AccountRepository : IAccountRepository
    {
        private List<Account> _accounts { get; set; }

        public AccountRepository()
        {
            _accounts = new List<Account>();

            _accounts.Add(new Account() 
            {
                Id = "13622871725",
                Name = "Igor Juliani",
                Balance = 1000,
                Limit = 10M
            });

            _accounts.Add(new Account()
            {
                Id = "13589741232",
                Name = "Joao Guimaraes",
                Balance = 100,
                Limit = 10M
            });
        }

        public decimal GetBalance(string id)
            => _accounts
            .FirstOrDefault(acc => acc.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .Balance;

        public Account GetAccount(string id)
           => _accounts
            .FirstOrDefault(acc => acc.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        public decimal WithdrawValue(string id, decimal value)
        {
            var client = _accounts
                .FirstOrDefault(acc => acc.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(client.Id))
                throw new AtivaBankException("Cliente nao localizado na base");

            var index = _accounts.IndexOf(client);

            client.Balance -= value;

            _accounts[index] = client;

            return client.Balance;
        }

        public decimal DepositValue(string id, decimal value)
        {
            var client = _accounts
                .FirstOrDefault(acc => acc.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(client.Id))
                throw new AtivaBankException("Cliente nao localizado na base");

            var index = _accounts.IndexOf(client);

            client.Balance += value;

            _accounts[index] = client;

            return client.Balance;
        }

        public void CreateAccount(Account newAccount) => _accounts.Add(newAccount);

        public bool TransferValue(decimal value, Account accountSender, Account accountReceiver)
        {
            Func<string, string, List<Account>, bool> locator = (acc1, acc2, list) =>
            {
                var locate = new List<Account>();

                foreach (var acc in list)
                {
                    if (acc.Id.Equals(acc1, StringComparison.InvariantCultureIgnoreCase) || acc.Id.Equals(acc2, StringComparison.InvariantCultureIgnoreCase))
                        locate.Add(acc);
                }

                if (locate.Count() == 2)
                    return true;

                return false;
            };

            var search = locator(accountSender.Id, accountReceiver.Id, _accounts);

            if (!search)
                throw new AtivaBankException("Clientes nao encontrados");

            if (accountSender.Balance >= value || accountSender.Limit >= value) 
            {
                var client1 = _accounts.IndexOf(accountSender);
                var client2 = _accounts.IndexOf(accountReceiver);

                accountReceiver.Balance += value;
                accountSender.Balance -= value;

                _accounts[client1] = accountSender;
                _accounts[client2] = accountReceiver;

                return true;
            }

            throw new AtivaBankException($"Cliente {accountSender.Id} sem saldo");

        }
    }
}