using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Models;
using SistemaGeneral.Services;

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

        private static async Task<ModelPermission?> GetPermissionAsync(PermissionService service, [FromRoute] short id) {
            return await service.GetPermissionAsync(id);
        }

        private static async Task<List<ModelPermission>?> GetPermissionsAsync(PermissionService service) {
            return await service.GetPermissionsAsync();
        }

        private static async Task<object?> AddPermissionAsync(PermissionService service, [FromBody] ModelPermissionDto model) {
            return await service.AddPermissionAsync(model);
        }

        private static async Task<object?> PatchPermissionAsync(PermissionService service, [FromBody] ModelPermission model) {
            return await service.PatchPermissionAsync(model);
        }

        private static async Task<bool> DeletePermissionAsync(PermissionService service, [FromRoute] byte id) {
            return await service.DeletePermissionAsync(id);
        }

        /************ ROLE_PERMISION ROUTES ************/

        private static async Task<List<ModelPermission>?> GetPermissionsByRoleAsync(PermissionService service, [FromRoute] byte id) {
            return await service.GetPermissionsByRoleAsync(id);
        }

        private static async Task<object?> AddRolePermissionAsync(RolePermissionService service, [FromBody] ModelRolePermission model) {
            return await service.AddRolePermissionAsync(model);
        }

        private static async Task<bool> DeleteRolePermissionAsync(RolePermissionService service, [FromBody] ModelRolePermission model) {
            return await service.DeleteRolePermissionAsync(model);
        }

    }
}
