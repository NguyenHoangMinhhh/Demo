// HomeController.cs
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Demo.Services;
using Microsoft.Extensions.Configuration;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private IWooService _wooService;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IWooService wooService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _wooService = wooService;
        }

        public async Task<IActionResult> Index()
        {
            var x = await  _wooService.GetProductsAsync();
            var y = await _wooService.GetProductByIdAsync(int .Parse("1"));
            var httpClient = _httpClientFactory.CreateClient();

            var apiConfig = _configuration.GetSection("WooCommerceApi").Get<WooCommerceApiConfig>();

            var response = await httpClient.GetStringAsync($"{apiConfig.Endpoint}products");

            // Kiểm tra xem dữ liệu trả về có phải là JSON không
            if (response.StartsWith("{") || response.StartsWith("["))
            {
                var products = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(response);
                return View(products);
            }
            else
            {
                // Xử lý trường hợp dữ liệu không phải là JSON
                // Ở đây bạn có thể log hoặc xử lý theo cách khác tùy thuộc vào yêu cầu của bạn.
                return View(new List<WooCommerceProduct>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}