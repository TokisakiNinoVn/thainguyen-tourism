// ApiResponse.cs
namespace BE.Helpers;

public class ApiResponse
{
    public int Code { get; set; }
    public string Status { get; set; } = "success";
    public string Message { get; set; } = "";
    public object? Data { get; set; }

    public static ApiResponse Success(object? data = null, string message = "Thành công")
    {
        return new ApiResponse
        {
            Code = 200,
            Status = "success",
            Message = message,
            Data = data
        };
    }

    public static ApiResponse Error(int code = 400, string message = "Lỗi xảy ra", object? data = null)
    {
        return new ApiResponse
        {
            Code = code,
            Status = "error",
            Message = message,
            Data = data
        };
    }
}
