using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrderDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;
        public OrdersProvider(OrderDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order() { Id = 1, CustomerId = 1, OrderDate = DateTime.Now, Total = (decimal)10.00, Items = new List<OrderItem>{new OrderItem{Id=1, ProductId = 1, Quantity= 100, UnitPrice=10, ProductName="A"} }});
                dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 2, OrderDate = DateTime.Now, Total = (decimal)20.00, Items = new List<OrderItem>{new OrderItem{Id=2, ProductId = 2, Quantity= 200, UnitPrice=20, ProductName="B"} }});
                dbContext.Orders.Add(new Db.Order() { Id = 3, CustomerId = 3, OrderDate = DateTime.Now, Total = (decimal)30.00, Items = new List<OrderItem>{new OrderItem{Id=3, ProductId = 3, Quantity= 300, UnitPrice=30, ProductName="C"} }});
                dbContext.Orders.Add(new Db.Order() { Id = 4, CustomerId = 4, OrderDate = DateTime.Now, Total = (decimal)40.00, Items = new List<OrderItem> { new OrderItem { Id = 4, ProductId = 4, Quantity = 400, UnitPrice = 40, ProductName = "D" } } });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order>? Orders, string? ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation("Querying customers");
                var orders = await dbContext.Orders.Include(x => x.Items).Where(x => x.Id == customerId).ToListAsync();
                if (orders != null)
                {
                    logger?.LogInformation("Customer found");
                    var result = mapper.Map<List<Db.Order>, List<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
