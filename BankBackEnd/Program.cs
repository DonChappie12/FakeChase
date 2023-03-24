using System.Text;
using BankBackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Todo: Have swagger have custom title, version, etc
// builder.Services.AddSwaggerGen(c =>
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank API", Version = "v1" })
// );

var connectionString = builder.Configuration.GetConnectionString("DBConnection");
var secretKey = builder.Configuration["JWT:Secret"];
builder.Services.AddDbContext<DBContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],

    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:Audience"],

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);

// Add Identity
builder.Services.AddDefaultIdentity<User>(options =>{
    options.SignIn.RequireConfirmedAccount = true;
    // options.Lockout.MaxFailedAccessAttempts = 10;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DBContext>()
.AddDefaultTokenProviders();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
    // options.TokenValidationParameters = new TokenValidationParameters()
    // {
    //     ValidateIssuerSigningKey = true,
    //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),

    //     ValidateIssuer = true,
    //     ValidIssuer = builder.Configuration["JWT:Issuer"],

    //     ValidateAudience = true,
    //     ValidAudience = builder.Configuration["JWT:Audience"],
    // };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

CreateRoles(builder.Services.BuildServiceProvider()).Wait();

app.MapControllers();

app.Run();

async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

    //** Basics of role but could add more roles
    string[] roleNames = {"Admin", "Manager", "Customer"};

    // ** Checks if role exists, if not it will create role
    foreach(string role in roleNames)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ** Seeds new Admin user if no user exist in Admin role
    var existingAdmin = await userManager.FindByEmailAsync("admin@admin.com");
    if (existingAdmin == null)
    {
        var newAdmin = new User
        {
            FirstName = "Admin",
            Email = "admin@admin.com",
            UserName = "admin@admin.com"
        };

        var createAdmin = await userManager.CreateAsync(newAdmin, "Admin2023!");
        if(createAdmin.Succeeded)
            await userManager.AddToRoleAsync(newAdmin, "Admin");
    }

    // ** Seeds new Manager user if no user exist in Manager role
    var existingManager = await userManager.FindByEmailAsync("manager@manager.com");
    if (existingManager == null)
    {
        var newManager = new User
        {
            FirstName = "Manager",
            Email = "manager@manager.com",
            UserName = "manager@manager.com"
        };

        var createManager = await userManager.CreateAsync(newManager, "Manager2023!");
        if(createManager.Succeeded)
            await userManager.AddToRoleAsync(newManager, "Manager");
    }
};
