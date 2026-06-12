using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Products.Commands.CreateProduct;
using InventoryManagement.Domain.Entities;
using Moq;

namespace InventoryManagement.Application.Tests.Products.Commands;

public sealed class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenCategoryExists_PersistsProductAndReturnsId()
    {
        var repositoryMock = new Mock<IProductCommandRepository>();
        repositoryMock
            .Setup(repository => repository.CategoryExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        repositoryMock
            .Setup(repository => repository.UnitOfMeasureExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        repositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Callback<Product, CancellationToken>((product, _) => product.Id = 15)
            .Returns(Task.CompletedTask);

        var handler = new CreateProductCommandHandler(repositoryMock.Object);
        var command = new CreateProductCommand(
            "Laptop",
            "12345678901234567890",
            "Equipo de prueba",
            "USD",
            3200m,
            4500m,
            8,
            2,
            "Activo",
            1,
            1);

        var productId = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(15, productId);
        repositoryMock.Verify(repository => repository.AddAsync(
            It.Is<Product>(product =>
                product.Name == "Laptop" &&
                product.Barcode == "12345678901234567890" &&
                product.Description == "Equipo de prueba" &&
                product.CurrencyCode == "USD" &&
                product.Status == "Activo" &&
                product.CategoryId == 1 &&
                product.UnitOfMeasureId == 1 &&
                product.PurchasePrice == 3200m &&
                product.SalePrice == 4500m),
            It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
