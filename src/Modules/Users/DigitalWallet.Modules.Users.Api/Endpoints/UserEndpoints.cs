using DigitalWallet.Modules.Users.Application.DTOs;
using DigitalWallet.Modules.Users.Application.Services;
using Microsoft.AspNetCore.Http;

namespace DigitalWallet.Modules.Users.Api.Endpoints;

public static class UserEndpoints
{
    public static async Task<IResult> HandleRegisterAsync(
        RegisterUserRequest request,
        UsersService usersService,
        CancellationToken ct)
    {
        var result = await usersService.RegisterUserAsync(request, ct);
        return Results.Created($"/api/users/{result.Id}", result);
    }
}
