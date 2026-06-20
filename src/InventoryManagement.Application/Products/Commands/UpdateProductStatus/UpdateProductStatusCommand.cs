using MediatR;

namespace InventoryManagement.Application.Products.Commands.UpdateProductStatus;

public sealed record UpdateProductStatusCommand(int Id, string Status) : IRequest;
