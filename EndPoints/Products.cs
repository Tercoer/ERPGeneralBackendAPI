using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using SistemaGeneral.Services;
using SistemaGeneral.Utility;

namespace SistemaGeneral.EndPoints {
    public static class Products {
        
        public static RouteGroupBuilder MapProductsEndPoints(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/products");
            group.MapGet("/", GetProducts);
            group.MapGet("/{id}", GetProductByID);
            group.MapPost("/{model}", AddProduct);
            group.MapPatch("{model}", PatchProduct);
            group.MapDelete("/{id}", DeleteProduct);
            return group;
        }
        
        public static async Task<IResult> GetProducts(ProductsService service) {
            IEnumerable<ModelProducts> products = await service.GetProductsAsync();
            return Validator.GetResult(products);
        }
        public static async Task<IResult> GetProductByID(ProductsService service, int id) {
            ModelProducts product = await service.GetProductByID(id);
            return Validator.GetResult(product);
        }

        public static async Task<IResult> AddProduct(ProductsService service, [FromBody] ModelProductsDto model) {
            bool products = await service.AddProductsAsync(model);
            return Validator.CreatedResult(products);
        }

        public static async Task<IResult> PatchProduct(ProductsService service, [FromBody] ModelProductsUpdate model) {
            bool isProductUpdated = await service.PatchProduct(model);
            return Validator.UpdatedResult(isProductUpdated);
        }


        public static async Task<IResult> DeleteProduct(ProductsService service, [FromRoute] int id) {
            bool isProductDeleted = await service.DeleteProduct(id);
            return Validator.DeletedResult(isProductDeleted);
        }

    }
}
