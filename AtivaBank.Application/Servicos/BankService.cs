using AtivaBank.Domain.Dto;
using AtivaBank.Domain.Entities;
using AtivaBank.Domain.Enum;
using AtivaBank.Domain.Exceptions;
using AtivaBank.Domain.Repositories;
using AtivaBank.Domain.Response;
using AtivaBank.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace AtivaBank.Application.Servicos
{
    public class BankService : IBankService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMovementRepository _movementRepository;

        private readonly ILogger<BankService> _logger;


        public BankService(IAccountRepository accountRepository, IMovementRepository movementRepository
            , ILogger<BankService> logger)
        {
            _accountRepo = accountRepository;
            _movementRepository = movementRepository;

            _logger = logger;
        }

        public Response<Account> CreateAccount(AccountDto newAccount)
        {
            _logger.LogInformation("Iniciando criacao de cliente");

            if (!string.IsNullOrEmpty(_accountRepo.GetAccount(newAccount.id).Id))
                throw new AtivaBankException("Conta ja cadastrada");

            var account = new Account()
            {
                Id = newAccount.id,
                Name = newAccount.name,
                Balance = 0,
                Limit = newAccount.limit,
            };

            _accountRepo.CreateAccount(account);

            _logger.LogInformation($"Cliente criado com sucesso - {account.ToString()}");

            return Response<Account>.SuccessResponse(account, "Conta Criada com Sucesso");
        }

        public Response<decimal> DepositValue(decimal value, string id)
        {
            try
            {
                _logger.LogInformation($"Iniciando deposito do cliente - {id}");

                if (string.IsNullOrEmpty(_accountRepo.GetAccount(id).Id))
                    throw new AtivaBankException("Conta nao encontrada");

                var newBalance = _accountRepo.DepositValue(id, value);

                var movement = new Movements()
                {
                    Id = Guid.NewGuid().ToString(),
                    AccountId = id,
                    Description = $"Lancamento de Deposito no valor de R${value}",
                    Amount = newBalance,
                    Type = MovementType.Credit,
                    Date = DateTime.Now,
                };

                _movementRepository.RegisterMovement(movement);

                _logger.LogInformation($"Deposito finalizado - {JsonConvert.SerializeObject(movement)}");

                return Response<decimal>.SuccessResponse(newBalance, "Deposito efetuado com Sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na tentativa de deposito - {id}");
                return Response<decimal>.ErrorResponse(default, ex.Message);
            }
        }

        public Response<decimal> WithdrawValue(decimal value, string id)
        {
            try
            {
                _logger.LogInformation($"Iniciando saque do cliente - {id}");

                var account = _accountRepo.GetAccount(id);

                if (string.IsNullOrEmpty(account.Id))
                    throw new AtivaBankException("Conta nao encontrada");

                if (account.Balance < value && account.Limit < value)
                    throw new AtivaBankException("Cliente sem saldo e limite para operacao");

                var newBalance = _accountRepo.WithdrawValue(id, value);

                var movement = new Movements()
                {
                    Id = Guid.NewGuid().ToString(),
                    AccountId = id,
                    Description = $"Lancamento de Saque no valor de R${value}",
                    Amount = newBalance,
                    Type = MovementType.Debit,
                    Date = DateTime.Now,
                };
                _movementRepository.RegisterMovement(movement);

                _logger.LogInformation($"Saque finalizado - {JsonConvert.SerializeObject(movement)}");

                return Response<decimal>.SuccessResponse(newBalance, "Retirada efetuada com Sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na tentativa de saque - {id}");
                return Response<decimal>.ErrorResponse(default, ex.Message);
            }
        }

        public Response<decimal> GetBalance(string id)
        {
            try
            {
                _logger.LogInformation($"Iniciando busca do saldo do cliente - {id}");

                if (string.IsNullOrEmpty(_accountRepo.GetAccount(id).Id))
                    throw new AtivaBankException("Conta nao encontrada");

                var balance = _accountRepo.GetBalance(id);

                _logger.LogInformation($"Busca finalizada - {id}");


                return Response<decimal>.SuccessResponse(balance, "Saldo Encontrado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na tentativa de busca do saldo - {id}");
                return Response<decimal>.ErrorResponse(default, ex.Message);
            }
        }

        public Response<bool> TransferToAccount(string accountId1, string accountId2, decimal value)
        {
            try
            {
                _logger.LogInformation($"Iniciando transferencia para o cliente - {accountId2}");

                var account1 = _accountRepo.GetAccount(accountId1);
                var account2 = _accountRepo.GetAccount(accountId2);

                var result = _accountRepo.TransferValue(value, account1, account2);

                for (var i = 0; i < 2; i++)
                {
                    var condition = i == 0;

                    _movementRepository.RegisterMovement(new Movements()
                    {
                        Id = Guid.NewGuid().ToString(),
                        AccountId = condition ? accountId1 : accountId2,
                        Description = condition ?
                            $"Lancamento de Transferencia no valor de -R${value}" :
                            $"Lancamento de Transferencia no valor de R${value}",
                        Amount = condition ? _accountRepo.GetBalance(accountId1) : _accountRepo.GetBalance(accountId2),
                        Type = condition ? MovementType.Debit : MovementType.Credit,
                        Date = DateTime.Now,
                    });
                }

                _logger.LogInformation($"Transferencia finalizada");

                return Response<bool>.SuccessResponse(result, "Tranferencia efetuado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na tentativa de transferencia");
                return Response<bool>.ErrorResponse(default, ex.Message);
            }
        }

        public Response<IEnumerable<Movements>> GetAccountStatement(StatementFilterDto filter)
        {
            try
            {
                _logger.LogInformation($"Iniciando busca do extrato - {JsonConvert.SerializeObject(filter)}");

                if (string.IsNullOrEmpty(_accountRepo.GetAccount(filter.id).Id))
                    throw new AtivaBankException("Conta nao encontrada");

                var movements = _movementRepository.GetAllMovements(filter.id);

                if (!string.IsNullOrEmpty(filter.initDate) && !string.IsNullOrEmpty(filter.endDate))
                {
                    movements = movements
                        .Where(movement => Convert.ToDateTime(filter.initDate) <= movement.Date
                        && Convert.ToDateTime(filter.endDate) >= movement.Date);
                }
                else if (!string.IsNullOrEmpty(filter.initDate))
                {
                    movements = movements
                        .Where(movement => Convert.ToDateTime(filter.initDate) <= movement.Date
                        && DateTime.Now >= movement.Date);
                }

                movements = movements.Where(movement => movement.Type.Equals(filter.type));

                _logger.LogInformation($"Busca finalizada - {JsonConvert.SerializeObject(filter)}");

                return Response<IEnumerable<Movements>>.SuccessResponse(movements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na tentativa de gerar o extrato - {JsonConvert.SerializeObject(filter)}");
                return Response<IEnumerable<Movements>>.ErrorResponse(Enumerable.Empty<Movements>(), ex.Message);
            }
        }
    }
}
