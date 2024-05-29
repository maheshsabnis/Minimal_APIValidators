
using FluentValidation;
using System.Text.Json;

namespace Minimal_APIValidators.CustomFilters
{
    public class ModelValidationFilter<TModel> : IEndpointFilter where TModel : class
    {
         
        private readonly IValidator<TModel> _validator;

        public ModelValidationFilter(IValidator<TModel> validator)
        {
            _validator = validator;
        }

        async ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                // The Stream Data
                Stream bodyData = context.HttpContext.Request.Body;

                if (bodyData.CanSeek)
                {
                    bodyData.Seek(0, System.IO.SeekOrigin.Begin);
                }
                // Deserialize the Data
                var request = await JsonSerializer.DeserializeAsync<TModel>(bodyData);
                // Validate
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    // If Invalid then Read Error Messages
                    var errors = validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.HttpContext.Response.WriteAsJsonAsync(errors);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

           return await next(context);

        }
    }
}
