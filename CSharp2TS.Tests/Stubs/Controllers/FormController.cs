using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase {
        [HttpPost]
        public ActionResult PostForm([FromForm] IFormCollection form) {
            return Ok();
        }
    }
}
