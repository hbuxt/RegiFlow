using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Identity;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Api.Features.Auth.Register
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            IUserService userService,
            IRoleService roleService,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _userService = userService;
            _roleService = roleService;
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
                _logger.LogInformation("Registration failed for user: {Email}. Validation errors occurred: {@Errors}", command.Email, validationResult.ToFormattedDictionary());
                return Result.Failure<Response>(validationResult.ToFormattedDictionary());
            }

            var existingUser = await _userService.GetAsync(command.Email);

            if (existingUser != null)
            {
                _logger.LogInformation("Registration failed for user: {Email}. User already exists", command.Email);
                return Result.Failure<Response>(Errors.AccountAlreadyExists());
            }

            var role = await _roleService.GetAsync(RoleNames.General, RoleScope.Application);

            if (role == null)
            {
                _logger.LogError("Registration failed for user: {Email}. Role not found", command.Email);
                return Result.Failure<Response>(Errors.RoleNotFound());
            }
            
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                CreatedAt = DateTime.UtcNow
            };
            
            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            try
            {
                user.HashedPassword = _passwordHasher.HashPassword(command.Password);
                
                _ = _dbContext.Users.Add(user);
                _ = _dbContext.UserRoles.Add(userRole);
                _ = await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for user: {UserEmail}. Unexpected error occurred", command.Email);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
            
            try
            {
                var accessToken = _tokenGenerator.GenerateAccessToken(user);
                    
                _logger.LogInformation("Registration succeeded for user: {UserId}", user.Id);
                return Result.Success(new Response()
                {
                    AccessToken = accessToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Registration succeeded for user: {UserId}. Access token failed to generate", user.Id);
                return Result.Success(new Response()
                {
                    AccessToken = null
                });
            }
        }
    }
}