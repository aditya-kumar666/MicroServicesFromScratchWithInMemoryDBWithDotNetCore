using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<Models.Product>? Products, string? ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, Models.Product? Product, string? ErrorMessage)> GetProductsAsync(int id);
    }
}
