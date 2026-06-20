using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public sealed class ProductCommandRepository : IProductCommandRepository
{
    private readonly ApplicationDbContext _context;

    public ProductCommandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        return _context.Products.AddAsync(product, cancellationToken).AsTask();
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Products.SingleOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        return _context.Categories.AnyAsync(category => category.Id == categoryId, cancellationToken);
    }

    public Task<bool> UnitOfMeasureExistsAsync(int unitOfMeasureId, CancellationToken cancellationToken)
    {
        return _context.UnitsOfMeasure.AnyAsync(unitOfMeasure => unitOfMeasure.Id == unitOfMeasureId, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
