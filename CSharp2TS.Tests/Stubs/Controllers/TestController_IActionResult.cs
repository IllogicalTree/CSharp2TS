using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/TestController")]
    public class TestController_IActionResult : ControllerBase {
        public TestController_IActionResult() {
        }

        [HttpGet]
        [TSEndpoint(typeof(string))]
        public IActionResult Get() {
            return Ok("Test API is working");
        }

        [HttpGet("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult GetById(int id) {
            return Ok(new TestClass());
        }

        [HttpGet("filtered")]
        [TSEndpoint(typeof(List<TestClass>))]
        public IActionResult GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            return Ok(new List<TestClass>());
        }

        [HttpPost]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult Create([FromBody] TestClass testClass) {
            return Ok(testClass);
        }

        [HttpPut("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult Update(int id, TestClass testClass) {
            return Ok(testClass);
        }

        [HttpPatch("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult PartialUpdate(int id, TestClass model) {
            return Ok(new TestClass());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            return NoContent();
        }
    }
}
