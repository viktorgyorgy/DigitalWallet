using DigitalWallet.Modules.Identity.Api.Filters;
using DigitalWallet.Modules.Identity.Application.DTOs;
using DigitalWallet.Modules.Identity.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DigitalWallet.Modules.Identity.Api.Endpoints;

public static class IdentityEndpoints
{
    private const string RefreshTokenCookieName = "refreshToken";

    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").WithTags("Auth");

        group.MapPost("register", async (
            [FromBody] RegisterRequest request,
            IdentityService identityService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await identityService.RegisterAsync(request, ct);

            return Results.Ok(result);
        })
        .AddEndpointFilter<ValidationFilter<RegisterRequest>>();

        group.MapPost("login", async (
            [FromBody] LoginRequest request,
            IdentityService identityService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await identityService.LoginAsync(request.Email, request.Password, ct);
            AppendRefreshTokenCookie(context, result.RefreshToken);

            return Results.Ok(new AuthResponse(result.AccessToken));
        })
        .AddEndpointFilter<ValidationFilter<LoginRequest>>();

        group.MapPost("refresh", async (
            HttpContext context,
            IdentityService identityService,
            CancellationToken ct) =>
        {
            var oldToken = context.Request.Cookies[RefreshTokenCookieName];
            if (string.IsNullOrEmpty(oldToken)) return Results.Unauthorized();

            var result = await identityService.RefreshAsync(oldToken, ct);
            AppendRefreshTokenCookie(context, result.RefreshToken);

            return Results.Ok(new AuthResponse(result.AccessToken));
        });

        group.MapPost("logout", async (
            HttpContext context,
            IdentityService identityService,
            CancellationToken ct) =>
        {
            var token = context.Request.Cookies[RefreshTokenCookieName];
            if (!string.IsNullOrEmpty(token))
            {
                await identityService.LogoutAsync(token, ct);
            }

            context.Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
            {
                Path = "/api/auth/refresh",
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            return Results.NoContent();
        });
    }

    private static void AppendRefreshTokenCookie(HttpContext context, string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/api/auth/refresh"
        };

        context.Response.Cookies.Append(RefreshTokenCookieName, token, options);
    }
}
