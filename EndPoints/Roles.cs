
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

        private static async Task<IResult> PatchRole(RoleService role, [FromBody] ModelRole model) {
            ModelRole? res = await role.PatchRoleAsync(model);
            if(res == null)
                return Results.NotFound();
            return Results.Ok(res);
        }

        private static async Task<IResult> AddRole(RoleService role, [FromBody] ModelRoleDto model) {
            ModelRole? result = await role.AddRoleAsync(model);
            if(result == null)
                return Results.NotFound();
            return Results.Ok(result);
        }

        private static async Task<IResult> GetRole(RoleService role, [FromRoute] byte id) {
            ModelRole? result = await role.GetRoleAsync(id);
            if(result == null)
                return Results.NotFound();
            return Results.Ok(result);
        }

        private static async Task<IResult> GetRoles(RoleService role) {
            IEnumerable<ModelRole> roles = await role.GetRolesAsync();
            if(!roles.Any())
                return Results.NotFound();
            return Results.Ok(roles);
        }
    }
}
