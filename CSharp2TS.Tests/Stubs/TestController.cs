using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs {
    [TSService]
    [ApiController]
    public class TestController : ControllerBase {
        [HttpGet]
        public string Get() {
            return "Hello, World!";
        }

        [HttpGet("{id}")]
        public TestClass GetWithId(Guid id) {
            return new TestClass();
        }

        [HttpGet("test")]
        public TestClass GetWithIdInParam(Guid id) {
            return new TestClass();
        }

        [HttpPost]
        public void Create(TestClassWithFolder model) {
        }

        [HttpPost("test/{id}")]
        public void CreateWithId(Guid id, TestClassWithFolder model) {
        }

        [HttpPut("test/{id}")]
        public void UpdateWithId(Guid id, TestClassWithFolder model) {
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id) {
        }

        [HttpGet("task-test")]
        public async Task GetTask() {
            await Task.Delay(1000);
        }

        [HttpGet("async-test")]
        public async Task<string> GetAsync() {
            await Task.Delay(1000);
            return "Hello, Async World!";
        }

        [HttpGet("async-test")]
        public ActionResult ActionResult() {
            return Ok("Hello, Async World!");
        }

        [HttpGet("async-test")]
        public ActionResult<string> ActionResultWithValue() {
            return "Hello, Async World!";
        }

        [HttpGet("async-test")]
        public IActionResult IActionResultWithValue() {
            return Ok("Hello, Async World!");
        }

        [HttpGet("async-test")]
        [TSEndpoint(typeof(TestClass))]
        public IActionResult TSEndpoint() {
            return Ok("Hello, Async World!");
        }
    }
}
