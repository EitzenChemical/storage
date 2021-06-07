using Microsoft.AspNetCore.Mvc;
using storage.Services;
using System.ComponentModel.DataAnnotations;

namespace storage.Controllers
{
    [ApiController]
    [Route("storage")]
    public class StorageController : ControllerBase
    {
        IDataService _dataService;
        public StorageController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("set_value")]
        public ActionResult Create([Required] string key, [Required] string value)
        {
            _dataService.Create(key, value);
            return Ok();
        }

        [HttpGet("get_value")]
        public JsonResult GetValue([Required] string key)
        {
            var value = _dataService.GetValue(key);
            return new JsonResult(value);
        }

        [HttpDelete("delete_value")]
        public ActionResult DeleteValue([Required] string key)
        {
            _dataService.DeleteValue(key);
            return Ok();
        }

        [HttpGet("get_keys")]
        public JsonResult GetKeys()
        {
            var result = _dataService.GetKeys();
            return new JsonResult(result);
        }
    }
}
