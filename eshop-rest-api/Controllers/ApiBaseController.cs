using Microsoft.AspNetCore.Mvc;

namespace eshop_rest_api.Controllers
{
    [ApiController]
    public abstract class ApiBaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;

        public ApiBaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}
