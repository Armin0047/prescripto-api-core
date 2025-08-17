using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Prescripto.DTOs;
using Prescripto.DTOs.TTACDtos;
using Prescripto.Models;
using Prescripto.Services;

namespace Prescripto.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class PharmcyController : Controller
    {
        private readonly IPharmcyService _service;
        public PharmcyController(IPharmcyService service)
        {
            _service = service;
        }

        [HttpGet("GetName")]        
        public IActionResult GetName()
        {
            CoNameDto dto = new CoNameDto();
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

                dto = _service.GetCoName(connInfo, connInfo.Username);
                if(!string.IsNullOrEmpty(dto.CoName))
                 return Ok(new { success = true, statusmessage = "" , data = dto });
                else
                return NotFound(new { success = false, statusmessage = "نام کاربری یا رمز عبور اشتباه است", data = dto });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message  , data = dto });
            }
        }


        [HttpGet("GetExsitsNoskhe")]
        public IActionResult GetExsitsNoskhe(string WebTrackingCode, string ShDaftarcheh, string DateFac)
        {
            ExistsNoskhe noskhe = new ExistsNoskhe();
            try
            {
                DateFac = DateFac.Replace("/", "");

                var headers = Request.Headers;

                var connInfo = new DbConfig
                {
                    Server = headers["X-Db-Server"],
                    Database = headers["X-Db-Name"],
                    Username = headers["X-Db-User"],
                    Password = headers["X-Db-Pass"]
                };         
                noskhe =_service.ExistsNoskhe(connInfo, WebTrackingCode, ShDaftarcheh, DateFac);
                if (!string.IsNullOrEmpty(noskhe.Sh_noskhe.ToString()))
                {
                    return Ok(new { success = true, statusmessage = "", data = noskhe });
                }
                else
                    return NotFound(new { success = false, statusmessage = "داده ای یافت نشد", data = noskhe });
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, statusmessage = ex.Message, data = noskhe });
            }


        }


        [HttpGet("GetSettingMojodi")]
        public IActionResult GetSettingMojodi()
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
                var val = _service.GetSettingMojodi(connInfo);
                if (!string.IsNullOrEmpty(val.ToString()))
                {
                    return Ok(new { success = true, statusmessage = "", data = val });
                }
                else
                    return NotFound(new { success = false, statusmessage = "داده ای یافت نشد", data = 0 });
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, statusmessage = ex.Message, data = 0 });
            }
        }


        [HttpGet("GetNameDr")]
        public IActionResult GetNameDr(string DocID)
        { 
            NameDr nameDr = new NameDr();   
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
                nameDr = _service.GetNameDr(connInfo,Convert.ToInt32(DocID));
                if (!string.IsNullOrEmpty(nameDr.NameDoctor))
                    return Ok(new { success = true, statusmessage = "", data = nameDr });
                else
                    return NotFound(new { success = false, statusmessage = "نام کاربری یا رمز عبور اشتباه است", data = nameDr });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = nameDr });
            }
        }

        [HttpPost("SelectKala")]
        public IActionResult SelectKala([FromBody] ListKalaNoskhe listKalaNoskhe)
        {
            List<RadifKala> radifKala = new List<RadifKala>();
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
                radifKala = _service.GetListKalaNoskhes(connInfo, listKalaNoskhe);
                //if (radifKala.Count() > 0)
                    return Ok(new { success = true, statusmessage = "", data = radifKala });
                //else
                //    return NotFound(new { success = false, statusmessage = "نام کاربری یا رمز عبور اشتباه است", data = nameDr });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = radifKala });
            }

        }

        [HttpGet("GetTakhasos")]
        public IActionResult GetTakhasos()
        {
            List<Takhasos> dto = new List<Takhasos>();
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

                dto = _service.GetListTakhasos(connInfo);
                //if (!string.IsNullOrEmpty(dto.CoName))
                    return Ok(new { success = true, statusmessage = "", data = dto });
                //else
                //    return NotFound(new { success = false, statusmessage = "نام کاربری یا رمز عبور اشتباه است", data = dto });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("InsertDr")]
        public IActionResult InsertDr(string Codedr, string NameDr, int codetakh)
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

                var dto = _service.InsertDoctor(connInfo, Codedr, NameDr, codetakh);
                if(Convert.ToInt32(dto) > 0)
                {
                    return Ok(new { success = true, statusmessage = "", data = (new { codetakh2 = dto }) });
                }
                else
                    return NotFound(new { success = false, statusmessage = "", data = (new { codetakh2 = 0 }) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = (new { codetakh2 = 0 }) });
            }
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(string CodeSazeman, int page = 1, int size = 50, string search = "")
        {

            List<KalaSmal> dto = new List<KalaSmal>();
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

                dto =_service.kalaSmal(connInfo, CodeSazeman, page ,  size,  search);
                //if (!string.IsNullOrEmpty(dto.CoName))
                return Ok(new { success = true, statusmessage = "", data = dto });
                //else
                //    return NotFound(new { success = false, statusmessage = "نام کاربری یا رمز عبور اشتباه است", data = dto });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("GetFard")]
        public IActionResult GetFard()
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

                dto = _service.fards(connInfo);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("GetSettingCreateKala")]
        public IActionResult GetSettingCreateKala(string Hname)
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

                var dto = _service.GetSettingKala(connInfo, Hname);
                return Ok(new { success = true, statusmessage = "", data = new { Val = dto} });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = "" });
            }
        }

        [HttpGet("CheckCodeKala")]
        public IActionResult CheckCodeKala(string Code)
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

                var dto = _service.GetCheckCodeKala(connInfo, Code);
                return Ok(new { success = true, statusmessage = "", data = new { Val = dto } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = "" });
            }
        }

        [HttpGet("GetDastedaroo")]
        public IActionResult GetDastedaroo()
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

                dto = _service.dastedaroo(connInfo);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("GetDrugGroup")]
        public IActionResult GetDrugGroup()
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

                dto = _service.druggroup(connInfo);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }


        [HttpGet("GetNoeKala")]
        public IActionResult GetNoeKala()
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

                dto = _service.noekala(connInfo);
                return Ok(new { success = true, statusmessage = "", data = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = dto });
            }
        }

        [HttpGet("GetLastCodeKala")]
        public IActionResult GetLastCodeKala()
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

                var dto = _service.LastCodeKala(connInfo);
                return Ok(new { success = true, statusmessage = "", data = new { code = dto} });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = "" });
            }
        }

        [HttpGet("GetCodeKalaWithCodeGroup")]
        public IActionResult GetCodeKalaWithCodeGroup(int codegroup)
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

                var dto = _service.codekalawithCodegroup(connInfo, codegroup);
                return Ok(new { success = true, statusmessage = "", data = new { code = dto } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, statusmessage = ex.Message, data = "" });
            }
        }

        [HttpPost("CreateKala")]
        public IActionResult CreateKala([FromBody] kala kala)
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
                var result = _service.CreateKala(connInfo, kala);
                return Ok(new { success = true, statusmessage = "", data = new { result = true} });
            }
            catch (Exception ex)
            {
                // همیشه با ساختار JSON پاسخ خطا بده
                return BadRequest(new { success = false, statusmessage = ex.Message, data = "" });
            }
        }

    }
}
