using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Infrastructure.Identity;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Api.Features.Auth.Login
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            IUserService userService,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _userService = userService;
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
                _logger.LogInformation("Login failed for user: {Email}. Validation errors occurred: {@Errors}", command.Email, validationResult.ToFormattedDictionary());
                return Result.Failure<Response>(validationResult.ToFormattedDictionary());
            }

            var user = await _userService.GetAsync(command.Email);

            if (user == null)
            {
                _logger.LogInformation("Login failed for user: {Email}. User not found", command.Email);
                return Result.Failure<Response>(Errors.InvalidCredentials());
            }

            try
            {
                if (!_passwordHasher.VerifyPassword(command.Password, user.HashedPassword))
                {
                    _logger.LogInformation("Login failed for user: {Email}. Invalid password", command.Email);
                    return Result.Failure<Response>(Errors.InvalidCredentials());
                }

                _logger.LogInformation("Login succeeded for user: {UserId}", user.Id);
                return Result.Success(new Response()
                {
                    AccessToken = _tokenGenerator.GenerateAccessToken(user)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user: {Email}. Unexpected error occurred", command.Email);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}