using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Models;
using SistemaGeneral.Services;
using SistemaGeneral.Utility;
using System.Collections;

namespace SistemaGeneral.EndPoints {
    public static class Permissions {

        public static RouteGroupBuilder MapPermissionEndpoints(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/permissions");
            group.MapGet("/{id}", GetPermissionAsync);
            group.MapGet("/", GetPermissionsAsync);
            group.MapPost("/{model}", AddPermissionAsync);
            group.MapPatch("/{model}", PatchPermissionAsync);
            group.MapDelete("/{id}", DeletePermissionAsync);

            group.MapGet("/by-role/{id}", GetPermissionsByRoleAsync);
            group.MapPost("/by-role/{model}", AddRolePermissionAsync);
            group.MapDelete("/by-role/{model}", DeleteRolePermissionAsync);

            return group;
        }


        /************ PERMISION ROUTES ************/

        private static async Task<IResult> GetPermissionAsync(PermissionService service, [FromRoute] short id) {
            IEnumerable<ModelPermission> permisions = await service.GetPermissionAsync(id);
            return Validator.GetResult(permisions);
        }

        private static async Task<IResult> GetPermissionsAsync(PermissionService service) {
            IEnumerable<ModelPermission> permisions = await service.GetPermissionsAsync();
            return Validator.GetResult(permisions);
        }

        private static async Task<IResult> AddPermissionAsync(PermissionService service, [FromBody] ModelPermissionDto model) {
            bool isPermissionAdded = await service.AddPermissionAsync(model);
            return Validator.CreatedResult(isPermissionAdded);
        }

        private static async Task<IResult> PatchPermissionAsync(PermissionService service, [FromBody] ModelPermission model) {
            bool isPermissionUpdated = await service.PatchPermissionAsync(model);
            return Validator.UpdatedResult(isPermissionUpdated);
        }

        private static async Task<IResult> DeletePermissionAsync(PermissionService service, [FromRoute] byte id) {
            bool isPermissionDeleted = await service.DeletePermissionAsync(id);
            return Validator.DeletedResult(isPermissionDeleted);
        }

        /************ ROLE_PERMISION ROUTES ************/

        private static async Task<IResult> GetPermissionsByRoleAsync(PermissionService service, [FromRoute] byte id) {
            IEnumerable permissionsByRole = await service.GetPermissionsByRoleAsync(id);
            return Validator.GetResult(permissionsByRole);
        }

        private static async Task<IResult> AddRolePermissionAsync(RolePermissionService service, [FromBody] ModelRolePermission model) {
            bool isRolePermissionAdded = await service.AddRolePermissionAsync(model);
            return Validator.CreatedResult(isRolePermissionAdded);
        }

        private static async Task<IResult> DeleteRolePermissionAsync(RolePermissionService service, [FromBody] ModelRolePermission model) {
            bool isRolePermissionDeleted = await service.DeleteRolePermissionAsync(model);
            return Validator.DeletedResult(isRolePermissionDeleted);
        }

    }
}
