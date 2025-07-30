using ApptManager.Mapper;
using ApptManager.Mapping;
using ApptManager.Middleware;
using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using ApptManager.Repo;
using ApptManager.Repo.Services;
using ApptManager.Services;
using ApptManager.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration().WriteTo.File("C:\\OneDrive - H&R BLOCK LTD\\Documents\\MyProject\\MyProject\\ApptManager\\ApptManager\\log\\log.txt",rollingInterval:RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();

// 🔹 Add Controllers & JSON Enum Support
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// 🔹 Mail Settings Configuration
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

// 🔹 Database Context & Connection
builder.Services.AddTransient<DapperDBContext>();
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var context = provider.GetRequiredService<DapperDBContext>();
    return context.CreateConnection();
});

// 🔹 Generic Repository Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 🔹 Repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IBookingRepo, BookingRepo>();
builder.Services.AddScoped<ITaxProfessionalRepo, TaxProfessionalRepo>();
builder.Services.AddScoped<ISlotRepo, SlotRepo>();

// 🔹 Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ITaxProfessionalService, TaxProfessionalService>();
builder.Services.AddScoped<ISlotService, SlotService>();

// 🔹 Other
builder.Services.AddScoped<SlotMapper>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<JwtTokenService>();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<RefreshTokenService>();


// 🔹 Swagger (only in dev)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// 🔹 JWT Auth Configuration
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

var app = builder.Build();

// 🔹 Swagger UI for dev only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Global Exception Handling
app.UseMiddleware<ExceptionMiddleware>(); // Global exception handler


app.UseHttpsRedirection();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None, // ✅ Allow cross-origin cookies
    Secure = CookieSecurePolicy.Always         // ✅ Send only over HTTPS in prod
});
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();