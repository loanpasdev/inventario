using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models;

public sealed class CreateProductInputModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(120, ErrorMessage = "El nombre no puede exceder 120 caracteres.")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código de barra es obligatorio.")]
    [RegularExpression(@"^\d{20}$", ErrorMessage = "El código de barra debe tener exactamente 20 dígitos.")]
    [Display(Name = "Código de barra")]
    public string Barcode { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Seleccione una moneda.")]
    [RegularExpression("^(USD|VES)$", ErrorMessage = "Seleccione una moneda válida.")]
    [Display(Name = "Moneda base")]
    public string CurrencyCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio de compra es obligatorio.")]
    [Range(0.01, 999999, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
    [Display(Name = "Precio de compra")]
    public decimal? PurchasePrice { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0.01, 999999, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
    [Display(Name = "Precio de venta")]
    public decimal? SalePrice { get; set; }

    [Range(0, 999999, ErrorMessage = "El stock debe ser 0 o mayor.")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Range(0, 999999, ErrorMessage = "El stock mínimo debe ser 0 o mayor.")]
    [Display(Name = "Stock mínimo")]
    public int MinimumStock { get; set; }

    [Required(ErrorMessage = "El estatus es obligatorio.")]
    [StringLength(20, ErrorMessage = "El estatus no puede exceder 20 caracteres.")]
    [Display(Name = "Estatus")]
    public string Status { get; set; } = "Activo";

    [Required(ErrorMessage = "Seleccione una categoría.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
    [Display(Name = "Categoría")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Seleccione una unidad de medida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una unidad de medida.")]
    [Display(Name = "Unidad de medida")]
    public int? UnitOfMeasureId { get; set; }
}
