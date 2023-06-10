using ECommerce.Api.Search.Interfaces;
using System.Linq;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic? SearchResults)> SearchAsync(int customerId)
        {
            var orderResult = await orderService.GetOrdersAsync(customerId);
            var productResult = await productService.GetProductAsync();
            var customersResult = await customerService.GetCustomerAsync(customerId);
            if (orderResult.IsSuccess)
            {
                foreach (var item in from order in orderResult.Orders
                                     from item in order?.Items
                                     select item)
                {
                    item.ProductName = productResult.IsSuccess ? productResult.Products?.FirstOrDefault(p => p?.Id == item?.ProductId)?.Name : "Product information is not available.";
                }

                var result = new
                {
                    Customer = customersResult.IsSuccess ? customersResult.Customer : new {Name = "Customer information is not available"},
                    Order = orderResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
