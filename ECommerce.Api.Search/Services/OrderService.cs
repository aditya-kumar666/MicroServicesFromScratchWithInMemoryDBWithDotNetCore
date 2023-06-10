using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using System.Text.Json;

namespace ECommerce.Api.Search.Services
{
    public class OrderService : IOrderService
    {
        private IHttpClientFactory httpClientFactory;
        private ILogger<OrderService> logger;

        public OrderService(IHttpClientFactory httpClientFactory, ILogger<OrderService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Order>? Orders, string? ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("OrdersService");
                var response = await client.GetAsync($"api/orders/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
