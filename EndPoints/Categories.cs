
using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Models;
using SistemaGeneral.Services;
using SistemaGeneral.Utility;
using System.Collections;

namespace SistemaGeneral.EndPoints {
    public static class Categories {

        public static RouteGroupBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/categories");

            group.MapGet("/", GetCategories);
            group.MapGet("/{id:int}", GetCategory);
            group.MapPost("/", CreateCategory);
            group.MapPatch("/{id:int}", UpdateCategory);
            group.MapDelete("/{id:int}", DeleteCategory);
            return group;
        }

        private static async Task<IResult> GetCategories(CategoryService service) {
            IEnumerable category = await service.GetCategoriesAsync();
            return Validator.GetResult(category);
        }

        private static async Task<IResult> GetCategory(CategoryService service, [FromRoute]short id) {
            ModelCategory? category = await service.GetCategoryAsync(id);
            return Validator.GetResult(category);
        }

        private static async Task<IResult> CreateCategory(CategoryService service, [FromBody] ModelCategoryDTO model) {
            bool IsCategoryAdded = await service.CreateCategoryAsync(model);
            return Validator.CreatedResult(IsCategoryAdded);
        }

        private static async Task<IResult> UpdateCategory(CategoryService service, [FromRoute]short id, [FromBody] ModelCategory model) {
            if(model.ID != id) {
                return Results.BadRequest(new {
                    error = "Mismatched ID",
                    message = $"The ID from the route ({id}) doesn't matched with the Body ID ({model.ID})"
                });
            } 
            bool IsCategoryUpdated = await service.UpdateCategoryAsync(model);
            return Validator.UpdatedResult(IsCategoryUpdated);
        }

        private static async Task<IResult> DeleteCategory(CategoryService service, [FromRoute] short id){
            bool IsCategoryDeleted = await service.DeleteCategoryAsync(id);
            return Validator.DeletedResult(IsCategoryDeleted);
        }


    }


}
