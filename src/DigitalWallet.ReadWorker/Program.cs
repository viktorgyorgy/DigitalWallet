
using DigitalWallet.Read.Storage;
using DigitalWallet.ReadWorker.Projectors;
using DigitalWallet.Shared.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSharedInfrastructure();
builder.Services.AddMongoDatabase(builder.Configuration);


builder.Services.AddHostedService<UserProjector>();

var host = builder.Build();
host.Run();
