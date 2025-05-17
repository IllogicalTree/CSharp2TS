using CSharp2TS.Core.Attributes;
using CSharp2TS.Tests.Stubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs.Controllers {
    [TSService]
    [ApiController]
    [Route("api/Test")]
    public class AsyncIActionResult_TestController : ControllerBase {
        public AsyncIActionResult_TestController() {
        }

        [HttpGet]
        [TSEndpoint(typeof(string))]
        public async Task<IActionResult> Get() {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Get(int id) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Get(int id, int externalId) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<GenericClass1<TestClass2>> Generic() {
            throw new NotImplementedException();
        }

        [HttpGet("filtered")]
        [TSEndpoint(typeof(List<TestClass>))]
        public async Task<IActionResult> GetFiltered([FromQuery] string filter, [FromQuery] int limit = 10) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPost]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Create([FromBody] TestClass testClass) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPost]
        [TSEndpoint(typeof(string))]
        public async Task<IActionResult> CreateFromBody([FromBody] string model) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> Update(int id, TestClass testClass) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPatch("{id}")]
        [TSEndpoint(typeof(TestClass))]
        public async Task<IActionResult> PartialUpdate(int id, TestClass model) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWithTypedParam(int id) {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
