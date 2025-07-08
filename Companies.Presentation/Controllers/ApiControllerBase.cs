using Domain.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Companies.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        [NonAction]
        public ActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                // Kräver Microsoft.AspNetCore.Mvc.NewtonsoftJson
                ApiNotFoundResponse => NotFound(Results.Problem
                (
                    detail: ((ApiNotFoundResponse)baseResponse).Message,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not found",
                    instance: HttpContext.Request.Path
                    )),
                _ => throw new NotImplementedException()
            }; 
        }
    }
}
