namespace DigitalWallet.Modules.Users.Application.Services;

public class UsersService
{
    //private readonly IUserRepository _userRepository;
    //private readonly IUsersOutboxRepository _outboxRepository;
    //private readonly IPasswordHasher _passwordHasher;
    //private readonly IUsersUnitOfWork _unitOfWork;
    //private readonly IValidator<RegisterUserRequest> _validator;
    //private readonly TimeProvider _timeProvider;

    //public UsersService(IUserRepository userRepository,
    //    IUsersOutboxRepository outboxRepository,
    //    IPasswordHasher passwordHasher,
    //    IUsersUnitOfWork unitOfWork,
    //    IValidator<RegisterUserRequest> validator,
    //    TimeProvider timeProvider)
    //{
    //    _userRepository = userRepository;
    //    _outboxRepository = outboxRepository;
    //    _passwordHasher = passwordHasher;
    //    _unitOfWork = unitOfWork;
    //    _validator = validator;
    //    _timeProvider = timeProvider;
    //}

    //public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken ct = default)
    //{
    //    var validationResult = await _validator.ValidateAsync(request, ct);
    //    if (!validationResult.IsValid)
    //    {
    //        var errors = validationResult.Errors
    //            .GroupBy(e => e.PropertyName)
    //            .ToDictionary(
    //                g => g.Key,
    //                g => g.Select(x => x.ErrorMessage).ToArray()
    //            );

    //        throw new InvalidRequestException(errors);
    //    }

    //    var passwordHash = _passwordHasher.Hash(request.Password);

    //    var user = new User
    //    (
    //        request.Email,
    //        request.FirstName,
    //        request.LastName,
    //        passwordHash,
    //        _timeProvider.GetUtcNow().UtcDateTime
    //    );

    //    await _userRepository.AddAsync(user, ct);

    //    var integrationEvent = new UserCreatedIntegrationEvent(
    //        user.Id,
    //        user.Email,
    //        user.FirstName,
    //        user.LastName,
    //        Guid.NewGuid(),
    //        user.CreatedAt
    //    );

    //    var outboxMessage = OutboxMessage.FromEvent(integrationEvent, user.Id);

    //    await _outboxRepository.AddAsync(outboxMessage);

    //    await _unitOfWork.SaveChangesAsync(ct);

    //    return new RegisterUserResponse(user.Id, user.Email);
    //}
}
