using backend.Data;
using backend.Repository;
using backend.Repository.Impl;
using backend.Services;
using backend.Services.impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SuperHeroApi.Configuration;
using SuperHeroApi.Services;

var builder = WebApplication.CreateBuilder(args);

var tokenValidationParameters = new TokenValidationParameters() {
    ValidateLifetime = true,
    ValidateIssuer = false, // on production make it true
    ValidateAudience = false, // on production make it true
    RequireExpirationTime = true,
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero,
    ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
    ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
};

builder.Services.AddScoped<UserService, UserServiceImpl>();
//builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<JwtService, JwtServiceImpl>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkImpl>();
builder.Services.AddScoped<FoodRecordRepository, FoodRecordRepositoryImpl>();
builder.Services.AddScoped<FoodRecordsService, FoodRecordsServiceImpl>();
// Add services to the container.

// Newtonsoft library to serialize/deserialize json requests/responses
//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(opt => {
    opt.UseSqlServer(connectionString);
});

// Adding Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Adding authentication
builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
    options.Events = new JwtBearerEvents {
        OnAuthenticationFailed = context => {
            if(context.Exception.GetType() == typeof(SecurityTokenExpiredException)) {
                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("VueCorsPolicy", builder => {
        builder
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
          .WithOrigins("http://localhost:8080");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("VueCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Run();
