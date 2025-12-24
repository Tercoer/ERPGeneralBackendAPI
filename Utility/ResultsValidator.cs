using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace SistemaGeneral.Utility {
    public class ResultsValidator {

        public static IResult GetResult<T>(T resultFromDb) {
            if(resultFromDb == null)
                return Results.NotFound();            
            if(resultFromDb is IEnumerable enumerable && !(resultFromDb is string)) {
                if(!enumerable.Cast<object>().Any()) 
                    return Results.NotFound(resultFromDb);
            }
            return Results.Ok(resultFromDb);
        }
                
        public static IResult UpdatedResult(bool success) {
            return success ? Results.NoContent() : Results.NotFound();
        }

        public static IResult DeletedResult(bool success) {
            return success ? Results.NoContent() : Results.NotFound();
        }

        public static IResult CreatedResult(bool success) {
            if(success)
                return Results.Created();
            return Results.BadRequest(CreateProblem(
                    StatusCodes.Status400BadRequest,
                    "Fail creating resource"
            ));

        }

        private static ProblemDetails CreateProblem(int status, string title) {
            return new ProblemDetails {
                Status = status,
                Title = title
            };
        }
    }
}
