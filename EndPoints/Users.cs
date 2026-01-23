using Microsoft.AspNetCore.Mvc;
using SistemaGeneral.Services;
using SistemaGeneral.Models;
using System.Threading.Tasks;
using System.Collections;
using SistemaGeneral.Utility;

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

        private static async Task<IResult> AddUser(UserService service, [FromBody] ModelUserAddDto model) {
            bool isUserCreated = await service.AddUserAsync(model);
            return ResultsValidator.CreatedResult(isUserCreated);
        }

        private static async Task<IResult> GetUser(UserService service, [FromRoute] int id) {
            ModelUserInfoDto? user = await service.GetUserAsync(id);
            return ResultsValidator.GetResult(user);
        }

        private static async Task<IResult> GetUsers(UserService service) {
            IEnumerable<ModelUserInfoDto> users = await service.GetUsersAsync();
            return ResultsValidator.GetResult(users);
        }

        public static async Task<IResult> PatchUser(UserService users, [FromBody] ModelUserUpdateInputDto model) {
            bool isUserUpdated = await users.PatchUserAsync(model);
            return ResultsValidator.UpdatedResult(isUserUpdated);
        }

        public static async Task<IResult> PatchUserRole(UserService users, [FromBody] PatchModelUserRoleDto model) {
            bool isUserRoleUpdated = await users.PatchUserRoleAsync(model);
            return ResultsValidator.UpdatedResult(isUserRoleUpdated);
        }

        public static async Task<IResult> DeleteUser(UserService users, [FromRoute] int id) {
            bool isUserDeleted = await users.DeleteUserAsync(id);
            return ResultsValidator.DeletedResult(isUserDeleted);

        }

    }

}
