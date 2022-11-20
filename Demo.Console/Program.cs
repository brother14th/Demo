using ConsoleTables;
using Demo.Shared.ApiClient;
using Demo.Shared.Model;
using Demo.Shared.Service;
using Demo.Shared.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Console
{
    class Program
    {
        private static IConfiguration configuration;
        static async Task Main(string[] args)
        {
            ReadConfiguration();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            var services = serviceCollection.BuildServiceProvider();
            var orderService = services.GetService<IOrderService>();
            var productService = services.GetService<IProductService>();
            var topProductService = services.GetService<TopProductService>();
            var topKProduct = int.Parse(configuration["TopKProduct"] ?? "");
            List <OrderedProduct> inProgressOrderedProducts;

            try
            {
                inProgressOrderedProducts = await orderService.GetInProgressOrderedProducts();
                
                var topProducts = topProductService.GetTopProducts(topKProduct, inProgressOrderedProducts);
                
                System.Console.WriteLine($"Top {topKProduct} Products");
                ConsoleTable.From<OrderedProduct>(topProducts).Write();

                var topProduct = topProducts.FirstOrDefault();
                if (topProduct != null)
                {
                    try
                    {
                        int updateProductStock = int.Parse(configuration["UpdateProductStock"] ?? "");
                        await productService.UpdateStock(topProduct.MerchantProductNo, updateProductStock);
                        System.Console.WriteLine($"\nSuccess updating stock to {updateProductStock} for {topProduct.ProductName}!");
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine($"Error updating stock for {topProduct.ProductName}!");
                    }
                }
                else
                    System.Console.WriteLine("No product to update stock!");
            }
            catch (Exception)
            {
                System.Console.WriteLine("Error retrieving top products!");
            }
        }

        private static void ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false);
            configuration = builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {

            //Configure IChannelEngineApiClient 
            services.AddHttpClient<IChannelEngineApiClient, ChannelEngineApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiBaseUrl"]);
                client.DefaultRequestHeaders.Add("X-CE-KEY", configuration["ApiKey"]);
            });

            //Configure OrderService
            services.AddScoped<IOrderService, OrderService>();

            //Configure TopProductService
            services.AddScoped<TopProductService, TopProductService>();

            //Configure ProductService
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
