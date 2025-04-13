//file: Program.cs
using BE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// ======= Swagger + Controllers =======
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======= Load biến môi trường từ .env =======
DotNetEnv.Env.Load();
Console.WriteLine($"JWT_KEY: {Environment.GetEnvironmentVariable("JWT_KEY")}");

// ======= Đọc JWT từ biến môi trường và kiểm tra null =======
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("Thiếu JWT_KEY, JWT_ISSUER hoặc JWT_AUDIENCE trong file .env");
}

// ======= Kết nối Database =======
var connectionString =
    $"server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
    $"port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"user={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"password={Environment.GetEnvironmentVariable("DB_PASS")};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:8080", "http://localhost:8081")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// ======= JWT Authentication =======
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

// ======= Middleware Xử lý Exception Toàn Cục =======
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";

            if (ex is UnauthorizedAccessException || ex is SecurityTokenException)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    code = 401,
                    status = "unauthorized",
                    message = "Lỗi xác thực hoặc hết hạn phiên. Vui lòng đăng nhập lại.",
                    data = (object?)null
                });
            }
            else
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    code = 500,
                    status = "error",
                    message = "Lỗi hệ thống: " + ex.Message,
                    data = (object?)null
                });
            }
        }
        else
        {
            Console.WriteLine("Không thể ghi lỗi vì Response đã gửi: " + ex.Message);
        }
    }
});

// ======= Các Middleware chính =======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// builder.Services.AddSwaggerGen(options =>
// {
//     options.SupportNonNullableReferenceTypes();
// });
app.UseDeveloperExceptionPage();
app.UseExceptionHandler("/error");

// app.UseStaticFiles();
app.UseCors();
var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".geojson"] = "application/geo+json";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


// ======= Status Code Pages =======
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    var requestPath = context.HttpContext.Request.Path;

    if (response.HasStarted) return;

    response.ContentType = "application/json";

    var result = response.StatusCode switch
    {
        403 => new
        {
            code = 403,
            status = "error",
            message = "Bạn không có quyền truy cập trang quản lý.",
            data = (object?)null
        },
        404 => new
        {
            code = 404,
            status = "not_found",
            message = $"API không tồn tại: {requestPath}",
            data = (object?)null
        },
        405 => new
        {
            code = 405,
            status = "method_not_allowed",
            message = "Phương thức HTTP không được hỗ trợ cho endpoint này.",
            data = (object?)null
        },
        _ => new
        {
            code = response.StatusCode,
            status = "error",
            message = "Lỗi không xác định.",
            data = (object?)null
        }
    };

    await response.WriteAsJsonAsync(result);
});

// Middleware bọc response (nếu có)
app.UseMiddleware<BE.Middlewares.ResponseWrapperMiddleware>();
app.MapControllers();
app.Run();
