using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using FluentValidation;

namespace Back.Middlewares
{
    using System.Text.Json;

    using FluentValidation; // <--- Asegúrate de incluir este usando

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string title;
            string detail;
            IEnumerable<string> errors;

            switch (exception)
            {
                case ValidationException valEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Error de Validación";
                    detail = "Uno o más errores de validación ocurrieron.";
                    errors = valEx.Errors.Select(e => e.ErrorMessage);
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Recurso no encontrado";
                    detail = exception.Message;
                    errors = Enumerable.Empty<string>();
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = "Error Interno";
                    detail = "Ocurrió un error inesperado en el servidor.";
                    errors = Enumerable.Empty<string>();
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                ErrorList = errors // Cambiado a 'ErrorList' para evitar duplicidad de nombres en el tipo anónimo
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
