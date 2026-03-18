using DigitalWallet.Host;
using DigitalWallet.Read.Api;
using DigitalWallet.Read.Storage.Persistence;
using DigitalWallet.Shared.Api.Extensions;
using DigitalWallet.Shared.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1");
builder.Services.AddSharedInfrastructure();
builder.Services.AddWebInfrastructure(builder.Configuration);
builder.Services.RegisterModules(builder.Configuration);
builder.Services.AddReadSide(builder.Configuration);

var app = builder.Build();

if (args.Contains("--migrate"))
{
    app.RunModuleMigrations();

    using (var scope = app.Services.CreateScope())
    {
        var mongoInitializer = scope.ServiceProvider.GetRequiredService<MongoDbInitializer>();
        mongoInitializer.InitializeAsync().GetAwaiter().GetResult();
    }

    Console.WriteLine("Migrations successful. Container exiting.");
    return;
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/v1.json";

        options.WithTitle("Digital Wallet API")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    // Root redirect to the documentation
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

app.UseExceptionHandler();

var apiGroup = app.MapGroup("api");
apiGroup.MapModuleEndpoints();
apiGroup.MapReadEndpoints();

app.Run();
