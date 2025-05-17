// //file: middleware/VisitCounterMiddleware.cs

using System.Text.Json;
using System.Text.Json.Serialization;
namespace BE.Middlewares
{
    public class VisitCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string _counterFile = "visit-counter.json";
        private static readonly object _lock = new();

        public VisitCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            lock (_lock)
            {
                var now = DateTime.Now;
                var monthKey = $"count-{now:MMyyyy}";

                // Khởi tạo data
                Dictionary<string, int> counts;

                if (File.Exists(_counterFile))
                {
                    var json = File.ReadAllText(_counterFile);
                    try
                    {
                        counts = JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
                    }
                    catch
                    {
                        // Nếu file hỏng hoặc không đúng format
                        counts = new Dictionary<string, int>();
                    }
                }
                else
                {
                    counts = new Dictionary<string, int>();
                }

                // Tăng biến đếm của tháng hiện tại
                if (counts.ContainsKey(monthKey))
                    counts[monthKey]++;
                else
                    counts[monthKey] = 1;

                // Ghi lại file
                var options = new JsonSerializerOptions { WriteIndented = true };
                var updatedJson = JsonSerializer.Serialize(counts, options);
                File.WriteAllText(_counterFile, updatedJson);
            }

            await _next(context);
        }
    }

    public static class VisitCounterMiddlewareExtensions
    {
        public static IApplicationBuilder UseVisitCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VisitCounterMiddleware>();
        }
    }
}



// using System.Text;
// namespace BE.Middlewares
// {
//     public class VisitCounterMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private static readonly string _counterFile = "visit-counter.txt";
//         private static readonly object _lock = new();

//         public VisitCounterMiddleware(RequestDelegate next)
//         {
//             _next = next;
//         }

//         public async Task InvokeAsync(HttpContext context)
//         {
//             lock (_lock)
//             {
//                 int currentCount = 0;

//                 if (File.Exists(_counterFile))
//                 {
//                     var content = File.ReadAllText(_counterFile);
//                     int.TryParse(content, out currentCount);
//                 }

//                 currentCount++;
//                 File.WriteAllText(_counterFile, currentCount.ToString());
//             }

//             await _next(context);
//         }
//     }

//     public static class VisitCounterMiddlewareExtensions
//     {
//         public static IApplicationBuilder UseVisitCounter(this IApplicationBuilder builder)
//         {
//             return builder.UseMiddleware<VisitCounterMiddleware>();
//         }
//     }
// }
