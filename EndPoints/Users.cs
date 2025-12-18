using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Services;
using SistemaGeneral.Models;
using System.Threading.Tasks;

namespace SistemaGeneral.EndPoints {
    public static class Users {

        public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app) {
            RouteGroupBuilder group = app.MapGroup("/users");
            group.MapGet("/{id}", GetUser);
            group.MapGet("/", GetUsers);
            group.MapPost("/{model}", AddUser);
            group.MapPatch("/{model}", PatchUser);
            group.MapPatch("/user-role/{model}", PatchUserRole);
            group.MapDelete("/{id}", DeleteUser);

            return group;
        }

        private static async Task<object?> AddUser(UserService users, [FromBody] ModelUserAddDto model) {            
            return await users.AddUserAsync(model);
        }

        private static async Task<ModelUserInfoDto?> GetUser(UserService users, [FromRoute] int id) {
            return await users.GetUserAsync(id);
        }

        private static async Task<List<ModelUserInfoDto>?> GetUsers(UserService users) {
            return await users.GetUsersAsync();
        }

        public static async Task<object?> PatchUser(UserService users, [FromBody] ModelUser model) {
            return await users.PatchUserAsync(model);
        }

        public static async Task<object?> PatchUserRole(UserService users, [FromBody] PatchModelUserRoleDto model) {
            return await users.PatchUserRoleAsync(model);
        }

        public static async Task<bool> DeleteUser(UserService users, [FromRoute] int id) {
            return await users.DeleteUserAsync(id);
        }

    }

}
