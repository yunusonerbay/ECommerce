using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ECommerce.WebApi.Filters
{
    public class SwaggerResponseExamplesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Add response descriptions
            foreach (var response in operation.Responses)
            {
                switch (response.Key)
                {
                    case "200":
                        response.Value.Description = "Success";
                        break;
                    case "201":
                        response.Value.Description = "Created successfully";
                        break;
                    case "400":
                        response.Value.Description = "Bad request - validation error or invalid data";
                        break;
                    case "401":
                        response.Value.Description = "Unauthorized - authentication required";
                        break;
                    case "403":
                        response.Value.Description = "Forbidden - you don't have permission";
                        break;
                    case "404":
                        response.Value.Description = "Not found - resource doesn't exist";
                        break;
                    case "500":
                        response.Value.Description = "Server error - something went wrong on the server";
                        break;
                    case "502":
                        response.Value.Description = "Bad gateway - error communicating with upstream service";
                        break;
                    default:
                        if (string.IsNullOrEmpty(response.Value.Description))
                        {
                            response.Value.Description = "Response";
                        }
                        break;
                }
            }
        }
    }
}