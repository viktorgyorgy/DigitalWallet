namespace DigitalWallet.Modules.Users.Application.DTOs;

public record RegisterUserRequest(string Email, string FirstName, string LastName, string Password);
public record RegisterUserResponse(Guid Id, string Email);