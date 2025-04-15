using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/Test")]
    public class ActionResult_TestController : ControllerBase {
        public ActionResult_TestController() {
        }

        [HttpGet]
        public ActionResult<string> Get() {
            return "Test API is working";
        }

        [HttpGet("{id}")]
        public ActionResult<TestClass> Get(int id) {
            return new TestClass();
        }

        [HttpGet("{id}")]
        public ActionResult<TestClass> Get(int id, int externalId) {
            return new TestClass();
        }

        [HttpGet("filtered")]
        public ActionResult<IEnumerable<TestClass>> GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            return Ok(new List<TestClass>());
        }

        [HttpPost]
        public ActionResult<TestClass> Create([FromBody] TestClass testClass) {
            return testClass;
        }

        [HttpPost]
        public ActionResult<string> CreateFromBody([FromBody] string model) {
            return model;
        }

        [HttpPut("{id}")]
        public ActionResult<TestClass> Update(int id, TestClass testClass) {
            return testClass;
        }

        [HttpPatch("{id}")]
        public ActionResult<TestClass> PartialUpdate(int id, TestClass model) {
            return new TestClass();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            return NoContent();
        }
    }
}
