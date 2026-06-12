using InventoryManagement.Application.Currencies.Dtos;

namespace InventoryManagement.Application.Common.Interfaces;

public interface ICurrencyQueries
{
    Task<IReadOnlyList<CurrencyDto>> GetCurrenciesAsync(CancellationToken cancellationToken);
}
