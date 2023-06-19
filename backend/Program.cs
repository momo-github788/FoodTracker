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
using backend.Configuration;
using backend.Policies;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

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




builder.Services.AddScoped<AuthService, AuthServiceImpl>();
builder.Services.AddScoped<UserService, UserServiceImpl>();
builder.Services.AddScoped<RoleService, RoleServiceImpl>();
builder.Services.AddScoped<JwtService, JwtServiceImpl>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkImpl>();
builder.Services.AddScoped<EmailService, EmailServiceImpl>();
builder.Services.AddScoped<ConfirmationTokenService, ConfirmationTokenServiceImpl>();
builder.Services.AddScoped<ConfirmationTokenRepository, ConfirmationTokenRepositoryImpl>();
builder.Services.AddScoped<FoodRecordRepository, FoodRecordRepositoryImpl>();
builder.Services.AddScoped<FoodRecordsService, FoodRecordsServiceImpl>();
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x => {
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});
//builder.Services.AddTransient<IPasswordValidator<User>, PasswordPolicy>();

// Add services to the container.

builder.Services.AddControllers();
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

builder.Services.Configure<IdentityOptions>(opts => {
    opts.SignIn.RequireConfirmedEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireLowercase = false;
    opts.User.RequireUniqueEmail = true;

    opts.Password.RequireUppercase = false;
});

// Adding authentication
builder.Services.AddAuthentication(options => {
    //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})   
.AddJwtBearer(options => {
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
})
.AddCookie(options => {
    //options.LoginPath = "/api/auth/SignIn/Google";
})  
.AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
    options.Scope.Add("email");
    options.Scope.Add("profile");
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.SignInScheme = IdentityConstants.ExternalScheme;
});


builder.Services.AddAuthorization();


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

builder.Configuration.AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

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
app.AddGlobalErrorHandler();

app.Run();
