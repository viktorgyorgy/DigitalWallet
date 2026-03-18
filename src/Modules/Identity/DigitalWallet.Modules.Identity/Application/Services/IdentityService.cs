using DigitalWallet.Modules.Identity.Application.DTOs;
using DigitalWallet.Modules.Identity.Application.Interfaces;
using DigitalWallet.Modules.Identity.Contracts;
using DigitalWallet.Modules.Identity.Domain;
using DigitalWallet.Modules.Identity.Infrastructure.Persistence;
using DigitalWallet.Modules.Identity.Infrastructure.Persistence.Repositories;
using DigitalWallet.Shared.Application;

namespace DigitalWallet.Modules.Identity.Application.Services;

internal class IdentityService(
    CredentialRepository credentialRepo,
    RefreshTokenRepository tokenRepo,
    IdentityOutboxRepository outboxRepo,
    TokenService tokenService,
    IPasswordHasher passwordHasher,
    IdentityDbContext context)
{
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        // 1. Logic remains lean
        if (await credentialRepo.ExistsAsync(request.Email, ct))
        {
            throw new Exception("User already exists.");
        }

        var user = new Credential(request.Email, string.Empty);
        var hash = passwordHasher.Hash(user, request.Password);
        user.UpdatePassword(hash);

        await credentialRepo.AddAsync(user, ct);

        // 2. Map DTO directly to the Integration Event
        var @event = new UserIdentityCreatedIntegrationEvent(
            user.Id,
            user.Email,
            request.FirstName,
            request.LastName,
            Guid.NewGuid(),
            DateTimeOffset.UtcNow);

        var outboxMessage = OutboxMessage.FromEvent(@event, user.Id);
        await outboxRepo.AddAsync(outboxMessage);

        await context.SaveChangesAsync(ct);
        return new RegisterResponse(user.Id, user.Email);
    }

    public async Task<InternalAuthResult> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await credentialRepo.GetByEmailAsync(email, ct);

        if (user == null || !passwordHasher.Verify(user, password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return await CreateTokenPairAsync(user, ct);
    }

    public async Task<InternalAuthResult> RefreshAsync(string refreshTokenValue, CancellationToken ct = default)
    {
        var oldToken = await tokenRepo.GetByTokenValueAsync(refreshTokenValue, ct);

        if (oldToken == null || !oldToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        oldToken.Revoke();

        return await CreateTokenPairAsync(oldToken.Credential, ct);
    }

    public async Task LogoutAsync(string refreshTokenValue, CancellationToken ct = default)
    {
        var token = await tokenRepo.GetByTokenValueAsync(refreshTokenValue, ct);
        if (token != null)
        {
            token.Revoke();
            await context.SaveChangesAsync(ct);
        }
    }

    // --- Private Helper ---
    private async Task<InternalAuthResult> CreateTokenPairAsync(Credential user, CancellationToken ct)
    {
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshTokenValue = tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(refreshTokenValue, DateTime.UtcNow.AddDays(7), user.Id);

        await tokenRepo.AddAsync(refreshToken, ct);
        await context.SaveChangesAsync(ct);

        return new InternalAuthResult(accessToken, refreshTokenValue);
    }
}

internal record InternalAuthResult(string AccessToken, string RefreshToken);
