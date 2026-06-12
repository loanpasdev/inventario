using InventoryManagement.Application.Currencies.Dtos;
using MediatR;

namespace InventoryManagement.Application.Currencies.Queries.GetCurrencies;

public sealed record GetCurrenciesQuery : IRequest<IReadOnlyList<CurrencyDto>>;
