using Demo.Models;

namespace Demo.Services;

public interface IWooService
{
    Task<List<WooCommerceProduct>> GetProductsAsync();
    Task<WooCommerceProduct?> GetProductByIdAsync(int productId);
    Task<List<WooCommerceProduct>> SearchProductsAsync(string keyword);
    Task<int> CreateOrderAsync(OrderInfo orderInfo);
    Task<List<OrderInfo>> GetOrdersAsync();
    Task<OrderInfo> GetOrderByIdAsync(int orderId);
    Task<bool> UpdateOrderAsync(int orderId, OrderInfo updatedOrderInfo);
    Task<bool> DeleteOrderAsync(int orderId);
}