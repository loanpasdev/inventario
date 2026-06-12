using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public sealed class UserAuthRepository : IUserAuthRepository
{
    private readonly ApplicationDbContext _context;

    public UserAuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken)
    {
        var normalizedValue = usernameOrEmail.Trim().ToUpperInvariant();

        return _context.Users
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .SingleOrDefaultAsync(
                user => user.Username.ToUpper() == normalizedValue || user.Email.ToUpper() == normalizedValue,
                cancellationToken);
    }
}
