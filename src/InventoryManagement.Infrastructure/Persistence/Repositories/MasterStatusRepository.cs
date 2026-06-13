using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public sealed class MasterStatusRepository : IMasterStatusRepository
{
    private readonly ApplicationDbContext _context;

    public MasterStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Categories.SingleOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public Task<UnitOfMeasure?> GetUnitOfMeasureByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.UnitsOfMeasure.SingleOrDefaultAsync(unit => unit.Id == id, cancellationToken);
    }

    public Task<Currency?> GetCurrencyByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Currencies.SingleOrDefaultAsync(currency => currency.Id == id, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
