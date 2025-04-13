using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/TestController")]
    public class TestController_AsyncIActionResult : ControllerBase {
        public TestController_AsyncIActionResult() {
        }

        [HttpGet]
        [TSEndpoint(typeof(string))]
        public async Task<IActionResult> Get() {
            return await Task.FromResult(Ok("Test API is working"));
        }

        [HttpGet("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Get(int id) {
            return await Task.FromResult(Ok(new TestClass()));
        }

        [HttpGet("filtered")]
        [TSEndpoint(typeof(List<TestClass>))]
        public async Task<IActionResult> GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            return await Task.FromResult(Ok(new List<TestClass>()));
        }

        [HttpPost]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Create([FromBody] TestClass testClass) {
            return await Task.FromResult(Ok(testClass));
        }

        [HttpPost]
        [TSEndpoint(typeof(string))]
        public async Task<IActionResult> CreateFromBody([FromBody] string model) {
            return await Task.FromResult(Ok(model));
        }

        [HttpPut("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Update(int id, TestClass testClass) {
            return await Task.FromResult(Ok(testClass));
        }

        [HttpPatch("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> PartialUpdate(int id, TestClass model) {
            return await Task.FromResult(Ok(new TestClass()));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            return await Task.FromResult(NoContent());
        }
    }
}
