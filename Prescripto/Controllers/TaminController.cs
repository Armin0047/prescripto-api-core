using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Prescripto.DTOs;
using Prescripto.Models;
using Prescripto.Services;

namespace Prescripto.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/Pharmacy/[controller]")]
    public class TaminController : Controller
    {
        private readonly ITaminService _service;
        public TaminController(ITaminService service)
        {
            _service = service;
        }

        [HttpGet("GetSazeman")]
        public IActionResult GetSazeman()
        {
            List<Sazeman> sazeman = new List<Sazeman>();
            try
            {

                var headers = Request.Headers;

                var connInfo = new DbConfig
                {
                    Server = headers["X-Db-Server"],
                    Database = headers["X-Db-Name"],
                    Username = headers["X-Db-User"],
                    Password = headers["X-Db-Pass"]
                };
                sazeman = _service.Sazeman(connInfo);
                if (sazeman.Count>0)
                    return Ok(new { success = true, statusmessage = "", data = sazeman });
                else
                    return NotFound(new { success = false, statusmessage = "سازمانی یافت نشد", data = sazeman });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = sazeman });
            }
        }

        [HttpGet("UpdateKala")]
        public IActionResult UpdateKala(int codeKala, string CodeKalaNew, string Gencode)
        {
            try
            {
                var headers = Request.Headers;

                var connInfo = new DbConfig
                {
                    Server = headers["X-Db-Server"],
                    Database = headers["X-Db-Name"],
                    Username = headers["X-Db-User"],
                    Password = headers["X-Db-Pass"]
                };
                var dto = _service.updateKala(connInfo, codeKala, CodeKalaNew, Gencode);
                if(Convert.ToBoolean(dto))
                    return Ok(new { success = true, statusmessage = "" });
                else
                    return NotFound(new { success = false, statusmessage = "خطایی رخ داد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message });
            }
        }

        [HttpPost("InsertFactor")]
        public IActionResult InsertFactor([FromBody] ListKalaNoskhe taminHeder)
        {

            ExistsNoskhe noskhe = new ExistsNoskhe();
            try
            {

                var headers = Request.Headers;

                var connInfo = new DbConfig
                {
                    Server = headers["X-Db-Server"],
                    Database = headers["X-Db-Name"],
                    Username = headers["X-Db-User"],
                    Password = headers["X-Db-Pass"]
                };
                noskhe = _service.SabtNoskheTamin(connInfo, taminHeder);
                if (noskhe.Sh_noskhe > 0)
                    return Ok(new { success = true, statusmessage = "", data = noskhe });
                else
                    return NotFound(new { success = false, statusmessage = "خطایی رخ داد", data = noskhe });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = noskhe });
            }
        }
    }
}
