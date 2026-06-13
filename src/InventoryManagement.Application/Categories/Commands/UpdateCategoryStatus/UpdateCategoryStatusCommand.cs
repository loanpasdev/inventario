using MediatR;

namespace InventoryManagement.Application.Categories.Commands.UpdateCategoryStatus;

public sealed record UpdateCategoryStatusCommand(int Id, string Status) : IRequest;
