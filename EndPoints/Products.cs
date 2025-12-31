using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using SistemaGeneral.Services;
using SistemaGeneral.Utility;

namespace SistemaGeneral.EndPoints {
    public static class Products {
        
        public static RouteGroupBuilder MapProductsEndPoint(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/products");
            group.MapGet("/", GetProducts);
            group.MapPost("/{model}", AddProduct);

            return group;
        }
        
        public static async Task<IResult> GetProducts(ProductsService service) {
            IEnumerable<ModelProducts> products = await service.GetProductsAsync();
            return ResultsValidator.GetResult(products);
        }

        public static async Task<IResult> AddProduct(ProductsService service, [FromBody] ModelProductsDto model) {
            bool products = await service.AddProductsAsync(model);
            return ResultsValidator.CreatedResult(products);
        }





    }
}
