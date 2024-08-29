using Microsoft.AspNetCore.Mvc;

namespace HieuTM.API.B1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmptyCtrlsController : ControllerBase
    {
        [HttpGet("{numA}")]
        public object Test([FromRoute] int numA, int numB)
        {
            return Enumerable.Range(numA, numB);
        }

        [HttpPost("{numA}")]
        public object TestPost([FromRoute] int numA, [FromBody] int numB)
        {
            return Enumerable.Range(numA, numB);
        }

        [HttpPut("{numA}")]
        public object TestPut([FromRoute] int numA, [FromBody] int numB)
        {
            return Enumerable.Range(numA, numB);
        }

        [HttpDelete("{numA}")]
        public object TestDelete([FromRoute] int numA, [FromBody] int numB)
        {
            return Enumerable.Range(numA, numB);
        }
    }
}
