﻿using BasketApi.Domain;

namespace BasketApi.Infrastructure.Clients;

public interface IProductApiClient
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetAllProductsCached();
    Task<Order> GetOrder(string orderId);
    Task<Order> CreateOrder(CreateOrder order);
}
