using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Currencies.Dtos;
using MediatR;

namespace InventoryManagement.Application.Currencies.Queries.GetCurrencies;

public sealed class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, IReadOnlyList<CurrencyDto>>
{
    private readonly ICurrencyQueries _currencyQueries;

    public GetCurrenciesQueryHandler(ICurrencyQueries currencyQueries)
    {
        _currencyQueries = currencyQueries;
    }

    public Task<IReadOnlyList<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return _currencyQueries.GetCurrenciesAsync(cancellationToken);
    }
}
