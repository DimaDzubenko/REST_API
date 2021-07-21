using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace REST_API
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            ConsoleKeyInfo cki;

            for (; ; )
            {
                Console.WriteLine("To view products, press Enter or Escape to exit");
                cki = Console.ReadKey();

                if(cki.Key == ConsoleKey.Enter)
                {
                    var repositories = await ProcessRepositories();

                    Console.WriteLine("{0,-20} {1,5}\n", "Product name", "Category name");
                    foreach (var product in repositories.Products)
                    {
                        foreach (var category in repositories.Categories)
                        {
                            if (product.CategoryId == category.Id)
                                Console.WriteLine("{0,-20} {1,5}\n", product.Name, category.Name);
                        }
                    }
                }
                if (cki.Key == ConsoleKey.Escape)
                {
                    break;                    
                }
                    
            }
        }

        private static async Task<ProductCategoty> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync("https://tester.consimple.pro/");
            var repositories = await JsonSerializer.DeserializeAsync<ProductCategoty>(await streamTask);
            return repositories;
        }
    }

    public class ProductCategoty
    {
        [JsonPropertyName("Products")]
        public List<Product> Products { get; set; }
        [JsonPropertyName("Categories")]
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("CategoryId")]
        public int CategoryId { get; set; }
    }
}
