using HieuTM.API.B1.ViewModels.Validations;
using Microsoft.AspNetCore.Mvc;

namespace HieuTM.API.B1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationsController : ControllerBase
    {
        public ValidationsController()
        {
            ForValidation forValidation = new ForValidation();

            forValidation.Amount = 100;
            forValidation.Name = "Name";
        }

        [HttpPost]
        public IActionResult Post(ForValidation request)
        {
            return Ok(request);
        }
    }
}
