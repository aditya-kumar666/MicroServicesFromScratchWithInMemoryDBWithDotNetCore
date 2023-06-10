using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IProductService
    {
        Task<(bool IsSuccess, IEnumerable<Product>? Products, string? ErrorMessage)> GetProductAsync();
    }
}
