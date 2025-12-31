using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaGeneral.Services;
using SistemaGeneral.EndPoints;
using SistemaGeneral.Models;
using SistemaGeneral.Security;
using System.Security.Claims;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


// AddUsers services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c) => {
    c.SwaggerDoc("v1", new() { Title = "Mi API", Version = "Version #1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT como: Bearer {tu_token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {  
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddSingleton<DB>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<RolePermissionService>();
builder.Services.AddScoped<ProductsService>();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    string issuerSigningKey = JWT.PASSWORD_JWT ?? throw new InvalidOperationException("La clave JWT no está configurada correctamente."); ;

    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
    };
});
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler(errorApp => {
    errorApp.Run(async context => { 
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new {
            error = "Internal server error"
        });
    });
});



//app.MapGet("/prueba", Users.prueba);
app.MapGet("/ConexionDb", (DB db) => {
    UserService u = new UserService(db);    
    return u.conectarDB().ToString();
});

//Requiere estar autorizado
app.MapPost("/login", (DB db, [FromBody] ModelLogin model) => {
    JWT jwt = new JWT(db);
    return jwt.Login(model);
});

app.MapGet("/recursoPrivado", [Authorize](ClaimsPrincipal user) => {
    return Results.Ok("SOLO LOS QUE TIENEN TOKEN PUEDEN ACCEDER A ESTE ENDPOINT");    
});

app.MapUserEndpoints();
app.MapRoleEndpoints();
app.MapPermissionEndpoints();
app.MapProductsEndPoint();

app.Run();

/*########################        SIGUIENTES PASOS            #############################
 * CAMBIAR LA ESTRUCTURA A SIN TRY CATCH
 * app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Internal server error"
            });
        });
    });
 *
 *
 * Agregar el modulo de Products
 * 
 * 
 *########################        SIGUIENTES PASOS            #############################*/

