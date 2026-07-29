//using System.Net;
//using System.Text.Json;
//using Inventory.API.Helpers;

//namespace Inventory.API.Middleware
//{
//    public class ExceptionMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<ExceptionMiddleware> _logger;

//        public ExceptionMiddleware(
//            RequestDelegate next,
//            ILogger<ExceptionMiddleware> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message);

//                context.Response.ContentType = "application/json";
//                context.Response.StatusCode =
//                    (int)HttpStatusCode.InternalServerError;

//                var response = new ApiResponse<object>
//                {
//                    Success = false,
//                    Message = "An unexpected error occurred.",
//                    Data = null
//                };

//                var json = JsonSerializer.Serialize(response);

//                await context.Response.WriteAsync(json);
//            }
//        }
//    }
//}


using Inventory.API.Common;
using Inventory.API.Exceptions;
using Inventory.API.Helpers;
using System;
using System.Net;
using System.Text.Json;

namespace Inventory.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");

            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
             context.Response.StatusCode = (int)statusCode;
            //context.Response.StatusCode = statusCode switch
            //{
            //    ArgumentNullException => StatusCodes.Status400BadRequest,

            //    ArgumentException => StatusCodes.Status400BadRequest,

            //    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            //    KeyNotFoundException => StatusCodes.Status404NotFound,

            //    _ => StatusCodes.Status500InternalServerError
            //};

            var response = ApiResponse<object>.FailureResponse(message);

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}