
using DigitalWallet.Read.Storage;
using DigitalWallet.ReadWorker.Extensions;
using DigitalWallet.Shared.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSharedInfrastructure(
    builder.Configuration,
    rider => ReadWorkerExtensions.RegisterProjections(rider),
    (k, context) => ReadWorkerExtensions.ConfigureProjectionEndpoints(k, context, builder.Configuration)
);
builder.Services.AddMongoDatabase(builder.Configuration);

var host = builder.Build();
host.Run();
