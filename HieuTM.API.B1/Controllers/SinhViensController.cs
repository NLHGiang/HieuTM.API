using HieuTM.API.B1.Services;
using HieuTM.API.B1.ViewModels.SinhViens;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HieuTM.API.B1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinhViensController : ControllerBase
    {
        private readonly ISinhVienService _sinhVienService;

        public SinhViensController(
            ISinhVienService sinhVienService
        )
        {
            _sinhVienService = sinhVienService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _sinhVienService.GetList();

            return Ok(result);
        }

        // GET api/<SinhViensController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _sinhVienService.GetById(id);

            return Ok(result);
        }

        // POST api/<SinhViensController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateSinhVienVM request)
        {
            var result = _sinhVienService.Create(request);

            if (result)
                return Ok();

            return BadRequest(request);
        }

        // PUT api/<SinhViensController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateSinhVienVM request)
        {
            var result = _sinhVienService.Update(request);

            if (result)
                return Ok();

            return BadRequest(request);
        }

        // DELETE api/<SinhViensController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _sinhVienService.Delete(id);

            if (result)
                return Ok();

            return BadRequest(id);
        }
    }
}
