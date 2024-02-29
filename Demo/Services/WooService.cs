using Demo.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.Services 
{
    public class WooService : IWooService
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        

        public WooService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _endpoint = configuration["WooCommerceApi:Endpoint"] ?? "";
            _consumerKey = configuration["WooCommerceApi:ConsumerKey"] ?? throw new ArgumentNullException(nameof(httpClient));
            _consumerSecret = configuration["WooCommerceApi:ConsumerSecret"] ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_consumerKey}:{_consumerSecret}")));
        }

        public async Task<List<WooCommerceProduct>> GetProductsAsync()
        {
            var response = await _httpClient.GetStringAsync($"{_endpoint}products")!;
            var myDeserializedClass = JsonSerializer.Deserialize<List<WooResearch>>(response);

            return JsonSerializer.Deserialize<List<WooCommerceProduct>>(response)!;
        }

        public async Task<WooCommerceProduct?> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetStringAsync($"{_endpoint}products/{productId}");
            
            return JsonSerializer.Deserialize<WooCommerceProduct>(response);
        }

        public async Task<List<WooCommerceProduct>> SearchProductsAsync(string keyword)
        {
            var response = await _httpClient.GetStringAsync($"{_endpoint}products?search={keyword}")!;
            return JsonSerializer.Deserialize<List<WooCommerceProduct>>(response)!;
        }

        public async Task<int> CreateOrderAsync(OrderInfo orderInfo)
        {
            var orderJson = JsonSerializer.Serialize(orderInfo);
            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_endpoint}orders", content);

            if (response.IsSuccessStatusCode)
            {
                var createdOrder = JsonSerializer.Deserialize<WooCommerceOrderResponse>(await response.Content.ReadAsStringAsync());
                return createdOrder.Id;
            }

            return -1;
        }

        public async Task<List<OrderInfo>> GetOrdersAsync()
        {
            var response = await _httpClient.GetStringAsync($"{_endpoint}orders")!;
            return JsonSerializer.Deserialize<List<OrderInfo>>(response)!;
        }

        public async Task<OrderInfo> GetOrderByIdAsync(int orderId)
        {
            var response = await _httpClient.GetStringAsync($"{_endpoint}orders/{orderId}")!;
            return JsonSerializer.Deserialize<OrderInfo>(response)!;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, OrderInfo updatedOrderInfo)
        {
            var updatedOrderJson = JsonSerializer.Serialize(updatedOrderInfo);
            var content = new StringContent(updatedOrderJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_endpoint}orders/{orderId}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}orders/{orderId}");

            return response.IsSuccessStatusCode;
        }
    }
}