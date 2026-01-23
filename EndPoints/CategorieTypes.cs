
using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Models;
using SistemaGeneral.Services;
using SistemaGeneral.Utility;

namespace SistemaGeneral.EndPoints {
    public static class CategorieTypes {

        public static RouteGroupBuilder MapCategorieTypesEndpoints(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/categorieTypes");
            group.MapGet("/", GetCategorieTypes);
            group.MapGet("/{id:int}", GetCategorieType);
            group.MapPost("/{model}", CreateCategorieType);
            group.MapPatch("/{model}", UpdateCategorieType);
            group.MapDelete("/{id:int}", DeleteCategorieType);
            return group;
        }


        private static async Task<IResult> GetCategorieTypes(CategoryTypeService service) {
            IEnumerable<ModelCategoryType> result = await service.GetCategoryTypesAsync();
            return ResultsValidator.GetResult(result);
        }
              
        private static async Task<IResult> GetCategorieType(CategoryTypeService service, [FromRoute] short id) {
            ModelCategoryType? result = await service.GetCategoryTypeAsync(id);
            return ResultsValidator.GetResult(result);
        }
        
        private static async Task<IResult> CreateCategorieType(CategoryTypeService service, [FromBody] ModelCategoryTypeDTO model) {
            bool isCategoryTypeAdded = await service.CreateCategoryTypeAsync(model);
            return ResultsValidator.CreatedResult(isCategoryTypeAdded);
        }

        private static async Task<IResult> UpdateCategorieType(CategoryTypeService service, [FromBody]ModelCategoryType model) {
            bool IsCategoryTypeUpdated = await service.UpdateCategoryTypeAsync(model);
            return ResultsValidator.UpdatedResult(IsCategoryTypeUpdated);
        }
        private static async Task<IResult> DeleteCategorieType(CategoryTypeService service, [FromRoute] short id) {
            bool isCategorieTypeDeleted = await service.DeleteCategoryTypeAsync(id);
            return ResultsValidator.DeletedResult(isCategorieTypeDeleted);
        }
    }
}
