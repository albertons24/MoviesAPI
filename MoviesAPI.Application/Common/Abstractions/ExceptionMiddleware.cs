using FluentValidation;
using Microsoft.AspNetCore.Http;
using MoviesAPI.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Abstractions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException validationException)
            {
                var errors = validationException.Errors.Select(e => e.ErrorMessage);
                await HandleException(httpContext, errors, HttpStatusCode.BadRequest);
            }
            catch (NotFoundException notFoundException)
            {
                var errors = new List<string> { notFoundException.Message };
                await HandleException(httpContext, errors, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                var errors = new List<string> { "Internal server error." };
                await HandleException(httpContext, errors, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleException(HttpContext context, IEnumerable<string> errors, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { errors });
            return context.Response.WriteAsync(result);
        }
    }

}
