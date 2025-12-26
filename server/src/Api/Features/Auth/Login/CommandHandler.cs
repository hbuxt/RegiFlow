using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Infrastructure.Identity;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Auth.Login
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.ToFormattedDictionary();
                
                _logger.LogInformation("Login failed for user: {Email}. Validation errors occurred: {@Errors}", 
                    command.Email, validationErrors);
                return Result.Failure<Response>(validationErrors);
            }

            var user = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => !u.IsDeleted && u.Email == command.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed for user: {Email}. User not found", command.Email);
                return Result.Failure<Response>(Errors.InvalidCredentials());
            }

            if (!_passwordHasher.VerifyPassword(command.Password, user.HashedPassword))
            {
                _logger.LogWarning("Login failed for user: {Email}. Invalid password", command.Email);
                return Result.Failure<Response>(Errors.InvalidCredentials());
            }

            _logger.LogInformation("Login succeeded for user: {UserId}", user.Id);
            return Result.Success(new Response()
            {
                AccessToken = _tokenGenerator.GenerateAccessToken(user)
            });
        }
    }
}