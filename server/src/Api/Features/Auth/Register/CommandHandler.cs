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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Auth.Register
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            IUserService userService,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
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
                var validationErrors = validationResult.ToFormattedDictionary();
                
                _logger.LogInformation("Registration failed for user: {Email}. Validation errors occurred: {@Errors}", 
                    command.Email, validationErrors);
                return Result.Failure<Response>(validationErrors);
            }

            if (await _userService.ExistsAsync(command.Email))
            {
                _logger.LogInformation("Registration failed for user: {Email}. User already exists", command.Email);
                return Result.Failure<Response>(Errors.AccountAlreadyExists());
            }

            var role = await _dbContext.Roles
                .AsNoTracking()
                .SingleOrDefaultAsync(r => 
                    r.Scope == RoleScope.Application && 
                    r.Name == RoleNames.General);

            if (role == null)
            {
                _logger.LogError("Registration failed for user: {Email}. Role not found", command.Email);
                return Result.Failure<Response>(Errors.RoleNotFound());
            }
            
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                HashedPassword = _passwordHasher.HashPassword(command.Password),
                CreatedAt = DateTime.UtcNow
            };
            
            _ = _dbContext.Users.Add(user);
            
            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            
            _ = _dbContext.UserRoles.Add(userRole);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Registration succeeded for user: {UserId}", user.Id);
            return Result.Success(new Response()
            {
                AccessToken = _tokenGenerator.GenerateAccessToken(user)
            });
        }
    }
}