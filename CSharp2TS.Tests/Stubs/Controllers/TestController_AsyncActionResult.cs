using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/TestController")]
    public class TestController_AsyncActionResult : ControllerBase {
        public TestController_AsyncActionResult() {
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get() {
            return await Task.FromResult("Test API is working");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestClass>> GetById(int id) {
            return await Task.FromResult(new TestClass());
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<TestClass>>> GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            return await Task.FromResult(Ok(new List<TestClass>()));
        }

        [HttpPost]
        public async Task<ActionResult<TestClass>> Create([FromBody] TestClass testClass) {
            return await Task.FromResult(testClass);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateFromBody([FromBody] string model) {
            return await Task.FromResult(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TestClass>> Update(int id, TestClass testClass) {
            return await Task.FromResult(testClass);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TestClass>> PartialUpdate(int id, TestClass model) {
            return await Task.FromResult(new TestClass());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            return await Task.FromResult(NoContent());
        }
    }
}
