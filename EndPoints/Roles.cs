
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Models;
using SistemaGeneral.Services;

namespace SistemaGeneral.EndPoints {
    public static class Roles {
        public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/roles");
            group.MapGet("/{id}", GetRole);
            group.MapGet("/", GetRoles);
            group.MapPost("/{model}", AddRole);
            group.MapPatch("/{model}", PatchRole);
            group.MapDelete("/{id}", DeleteRole);

            return group;
        }

        private static async Task<bool> DeleteRole(RoleService role, [FromRoute] byte id) {
            return await role.DeleteRoleAsync(id);
        }

        private static async Task<object?> PatchRole(RoleService role, [FromBody] ModelRole model) {
            return await role.PatchRoleAsync(model);
        }

        private static async Task<object?> AddRole(RoleService role, [FromBody] ModelRoleDto model) {
            return await role.AddRoleAsync(model);
        }

        private static async Task<IResult?> GetRole(RoleService role, [FromRoute] byte id) {
            ModelRole? result = await role.GetRoleAsync(id);
            if(result == null)
                return Results.NotFound();
            return Results.Ok(result);
        }

        private static async Task<IResult?> GetRoles(RoleService role) {
            return await role.GetRolesAsync();
        }
    }
}
