using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/Test")]
    public class IActionResult_TestController : ControllerBase {
        public IActionResult_TestController() {
        }

        [HttpGet]
        [TSEndpoint(typeof(string))]
        public IActionResult Get() {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult Get(int id) {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public ActionResult<TestClass> Get(int id, int externalId) {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<GenericClass1<TestClass2>> Generic() {
            throw new NotImplementedException();
        }

        [HttpGet("filtered")]
        [TSEndpoint(typeof(List<TestClass>))]
        public IActionResult GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            throw new NotImplementedException();
        }

        [HttpPost]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult Create([FromBody] TestClass testClass) {
            throw new NotImplementedException();
        }

        [HttpPost]
        [TSEndpoint(typeof(string))]
        public IActionResult CreateFromBody([FromBody] string model) {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult Update(int id, TestClass testClass) {
            throw new NotImplementedException();
        }

        [HttpPatch("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult PartialUpdate(int id, TestClass model) {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
