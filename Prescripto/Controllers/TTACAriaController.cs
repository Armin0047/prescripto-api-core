using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;
using Prescripto.Services;

namespace Prescripto.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/pharmacy/[controller]")]
    public class TTACAriaController : Controller
    {

        private readonly ITTACAriaService _service;

        public TTACAriaController(ITTACAriaService service)
        {
            _service= service;
        }


        [HttpGet("Getfacgroups")]
        public IActionResult Getfacgroups()
        {
            List<Fard> dto = new List<Fard>();
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

                dto = _service.facgroups(connInfo);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpPost("SelectKala")]
        public IActionResult SelectKala([FromBody] List<ListTTAcKala> listTTAcKala)
        {
            List<TTAcKalaAria> dto = new List<TTAcKalaAria>();
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

                dto = _service.tTAcKalas(connInfo, listTTAcKala);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("GetAnbarList")]
        public IActionResult GetAnbarList()
        {
            List<Fard> dto = new List<Fard>();
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

                dto = _service.getAnbar(connInfo);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("UpdateKala")]
        public IActionResult UpdateKala(int codeKala, string irc)
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
                var dto = _service.updateKala(connInfo, codeKala, irc);
                if (Convert.ToBoolean(dto))
                    return Ok(new { success = true, statusmessage = "" });
                else
                    return NotFound(new { success = false, statusmessage = "خطایی رخ داد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message });
            }
        }

        [HttpPost("InsertIntoFacHeder")]
        public IActionResult InsertIntoFacHeder([FromBody] FachederFactor fachederFactor)
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

                var dto = _service.insertfactors(connInfo, fachederFactor);
                return Ok(new { success = true, statusmessage = "", data = new { code = dto} });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = "" });
            }
        }
    }
}
