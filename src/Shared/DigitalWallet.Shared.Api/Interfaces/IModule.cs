using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Api.Interfaces;

public interface IModule
{
    string Name { get; }
    void RegisterModule(IServiceCollection services, IConfiguration configuration);
    void MapEndpoints(IEndpointRouteBuilder endpoints);
    void ApplyMigrations(IApplicationBuilder app);
    void RegisterConsumers(IRiderRegistrationConfigurator rider);
    void ConfigureConsumerEndpoints(
        IKafkaFactoryConfigurator kafka,
        IRiderRegistrationContext context,
        IConfiguration configuration);
}
