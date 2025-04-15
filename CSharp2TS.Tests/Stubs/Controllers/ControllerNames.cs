using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [Route("api/custom-route")]
    [ApiController]
    [TSService]
    public class CustomRouteController : ControllerBase {
        [HttpGet]
        public ActionResult<string> Get() {
            return "Test API is working";
        }
    }

    [ApiController]
    [TSService]
    public class NoRouteController : ControllerBase {
        [HttpGet]
        public ActionResult<string> Get() {
            return "Test API is working";
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    [TSService]
    public class TemplatedRouteController : ControllerBase {
        [HttpGet]
        public ActionResult<string> Get() {
            return "Test API is working";
        }
    }
}
