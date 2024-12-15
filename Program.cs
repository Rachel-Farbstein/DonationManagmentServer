using Microsoft.EntityFrameworkCore;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using DonationManagmentServer.Repisotories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NuGet.Protocol;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DonationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<S3Service>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DonorService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<DonorRepository>();

// Add JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["AWS:Authority"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false, // ביטול אימות Audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["AWS:Authority"],
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully.");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge triggered: {context.AuthenticateFailure?.Message}");
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Headers.ContainsKey("Authorization"))
//    {
//        // קבלת הטוקן מכותרת Authorization
//        var authorizationHeader = context.Request.Headers["Authorization"].ToString();

//        if (string.IsNullOrEmpty(authorizationHeader))
//        {
//            Console.WriteLine("Authorization header is missing.");
//            return;
//        }

//        // הסרת 'Bearer ' מהתחלת הערך
//        var token = authorizationHeader.StartsWith("Bearer ")
//            ? authorizationHeader.Substring("Bearer ".Length).Trim()
//            : authorizationHeader;

//        Console.WriteLine($"Received token: {token}");

//        try
//        {
//            // קריאת הטוקן
//            var handler = new JwtSecurityTokenHandler();
//            if (!handler.CanReadToken(token))
//            {
//                Console.WriteLine("The token format is invalid.");
//                return;
//            }

//            var jwtToken = handler.ReadJwtToken(token);

//            //if (jwtToken != null)
//            //{
//            //    var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
//            //    context.User = new ClaimsPrincipal(identity);
//            //}

//            await next();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error while processing token: {ex.Message}");
//        }
//    }
//});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
