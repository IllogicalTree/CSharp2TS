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
    }
}
