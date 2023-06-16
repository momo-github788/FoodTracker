using backend.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace backend.Configuration {
    public class GlobalExceptionHandlerMiddleware {

        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                // If any error occurs with our requests in this middleware, it will be handled by catch block
                await _next(context);
            } catch (Exception ex) {
                // This method processes the errored request and identifies the type then provides an
                // appropriate and clean response back to the client
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex) {
            HttpStatusCode statusCode;
            var stackTrace = String.Empty;
            string message = String.Empty;
            var exceptionType = ex.GetType();

            if (exceptionType == typeof(NotFoundException)) {
                statusCode = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
                message = ex.Message;
            } else if (exceptionType == typeof(BadRequestException)) {
                statusCode = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace;
                message = ex.Message;
            } else if (exceptionType == typeof(Exceptions.UnauthorizedAccessException)) {
                statusCode = HttpStatusCode.Unauthorized;
                stackTrace = ex.StackTrace;
                message = ex.Message;
            } else if (exceptionType == typeof(AlreadyExistsException)) {
                statusCode = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace;
                message = ex.Message;
            } else if (exceptionType == typeof(InvalidTokenException)) {
                statusCode = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
                message = ex.Message; 
            } else {
                statusCode = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
                message = ex.Message;
            }

            context.Request.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            

            return context.Response.WriteAsJsonAsync(
                new {
                    Error = message, statusCode
                }
            );



        }

    }
}
