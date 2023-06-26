using AtivaBank.Application.Servicos;
using AtivaBank.Domain.Repositories;
using AtivaBank.Domain.Services;
using AtivaBank.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace AtivaBank.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
            => services.AddScoped<IBankService, BankService>();

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
            => services.AddSingleton<IAccountRepository, AccountRepository>()
                .AddSingleton<IMovementRepository, MovementRepository>();
    }
}