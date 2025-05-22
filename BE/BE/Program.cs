// file: Program.cs
using BE;
using BE.Middlewares;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========== Load biến môi trường ==========
Env.Load();

string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY chưa được cấu hình.");
string jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER chưa được cấu hình.");
string jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE chưa được cấu hình.");

// ========== Kết nối Database ==========
string connectionString =
    $"server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
    $"port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"user={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"password={Environment.GetEnvironmentVariable("DB_PASS")};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

// ========== Cấu hình JWT ==========
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
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

// ========== Cấu hình Kestrel ==========
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5124); // Lắng nghe từ tất cả IPs (cần thiết cho ngrok / docker)
    serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100MB
});
builder.WebHost.UseUrls("http://*:5124");

// ========== Cấu hình Form (multipart) ==========
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
});

// ========== Swagger, Controllers ==========
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== Logging ==========
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// ========== Logging Request ==========
app.Use(async (context, next) =>
{
    var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    Console.WriteLine($"[{now}] {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
    await next();
});

// ========== Middleware thứ tự quan trọng ==========
app.UseMiddleware<CorsMiddleware>();               // CORS phải trước Authentication
app.UseVisitCounter();                             // Đếm lượt truy cập

// ======= Exception handler ==========
app.Use(async (context, next) =>
{
    try
    {
        context.Response.OnStarting(() =>
        {
            Console.WriteLine("Origin: " + context.Request.Headers.Origin);
            Console.WriteLine("Access-Control-Allow-Origin: " + context.Response.Headers["Access-Control-Allow-Origin"]);
            return Task.CompletedTask;
        });

        await next();
    }
    catch (Exception ex)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                UnauthorizedAccessException or SecurityTokenException => 401,
                _ => 500
            };

            var errorResponse = new
            {
                code = context.Response.StatusCode,
                status = context.Response.StatusCode == 401 ? "unauthorized" : "error",
                message = context.Response.StatusCode == 401
                    ? "Lỗi xác thực hoặc hết hạn phiên. Vui lòng đăng nhập lại."
                    : "Lỗi hệ thống: " + ex.Message,
                data = (object?)null
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        else
        {
            Console.WriteLine("Không thể ghi lỗi vì response đã gửi: " + ex.Message);
        }
    }
});

// ========== Middleware chính ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider
    {
        Mappings = { [".geojson"] = "application/geo+json" }
    }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ========== StatusCode Pages ==========
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    var requestPath = context.HttpContext.Request.Path;

    if (response.HasStarted) return;

    response.ContentType = "application/json";
    var result = response.StatusCode switch
    {
        403 => new { code = 403, status = "error", message = "Bạn không có quyền truy cập trang quản lý.", data = (object?)null },
        404 => new { code = 404, status = "not_found", message = $"API không tồn tại: {requestPath}", data = (object?)null },
        405 => new { code = 405, status = "method_not_allowed", message = "Phương thức HTTP không được hỗ trợ cho endpoint này.", data = (object?)null },
        _ => new { code = response.StatusCode, status = "error", message = "Lỗi không xác định.", data = (object?)null }
    };

    await response.WriteAsJsonAsync(result);
});

// ========== Bọc response ==========
app.UseMiddleware<ResponseWrapperMiddleware>();

// ========== Map route ==========
app.MapControllers();
app.MapGet("/", () => Results.Json(new { code = 200, status = "success", message = "Thành công", data = "Hello World!" }));
app.MapGet("/error", () => Results.Json(new { code = 500, status = "error", message = "Lỗi hệ thống", data = (object?)null }));

// ========== Khởi chạy ==========
app.Run();
