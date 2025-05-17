using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/Test")]
    public class AsyncActionResult_TestController : ControllerBase {
        public AsyncActionResult_TestController() {
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get() {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestClass>> Get(int id) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestClass>> Get(int id, int externalId) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult<GenericClass1<TestClass2>>> Generic() {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<TestClass>>> GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<TestClass>> Create([FromBody] TestClass testClass) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateFromBody([FromBody] string model) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TestClass>> Update(int id, TestClass testClass) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TestClass>> PartialUpdate(int id, TestClass model) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetWithTypedParam(int id) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
