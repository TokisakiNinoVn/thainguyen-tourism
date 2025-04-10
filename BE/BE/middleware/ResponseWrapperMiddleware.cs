//file: ResponseWrapperMiddleware.cs
namespace BE.Middlewares;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System;

public class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseWrapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBody = context.Response.Body;

        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await _next(context);

            // Nếu không có dữ liệu ghi vào memStream → bỏ qua
            if (memStream.Length == 0)
            {
                context.Response.Body = originalBody;
                return;
            }

            memStream.Position = 0;
            string bodyText;

            try
            {
                bodyText = await new StreamReader(memStream).ReadToEndAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc body từ memStream: " + ex.Message);
                context.Response.Body = originalBody;
                return;
            }

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";

            // Nếu body rỗng hoặc null → không bọc
            if (string.IsNullOrWhiteSpace(bodyText))
            {
                context.Response.StatusCode = 204;
                return;
            }

            object? parsedBody;
            try
            {
                parsedBody = JsonSerializer.Deserialize<JsonElement>(bodyText);

                if (parsedBody is JsonElement element &&
                    element.TryGetProperty("code", out _) &&
                    element.TryGetProperty("status", out _) &&
                    element.TryGetProperty("message", out _) &&
                    element.TryGetProperty("data", out _))
                {
                    await context.Response.WriteAsync(bodyText);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi parse JSON trong middleware ResponseWrapper: " + ex.Message);
                parsedBody = bodyText;
            }

            var wrapped = new
            {
                code = context.Response.StatusCode,
                status = context.Response.StatusCode < 400 ? "success" : "error",
                message = GetMessage(context.Response.StatusCode),
                data = parsedBody
            };

            try
            {
                await context.Response.WriteAsJsonAsync(wrapped);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi WriteAsJsonAsync trong middleware: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            context.Response.Body = originalBody;

            if (context.Response.HasStarted)
            {
                Console.WriteLine("Lỗi sau khi response đã gửi: " + ex.Message);
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var wrappedError = new
            {
                code = 500,
                status = "error",
                message = "Lỗi hệ thống: " + ex.Message,
                data = (object?)null
            };

            try
            {
                await context.Response.WriteAsJsonAsync(wrappedError);
            }
            catch (Exception writeEx)
            {
                Console.WriteLine("Không thể ghi response lỗi: " + writeEx.Message);
                throw;
            }
        }
    }

    private static string GetMessage(int code)
    {
        return code switch
        {
            200 => "Thành công",
            201 => "Tạo thành công",
            400 => "Yêu cầu không hợp lệ",
            401 => "Không xác thực",
            403 => "Không có quyền",
            404 => "Không tìm thấy",
            _ => "Xảy ra lỗi"
        };
    }
}
