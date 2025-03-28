using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CSharp2TS.Tests.Stubs {
    [TSService]
    public class TestService {
        [HttpGet]
        public string Get() {
            return "Hello, World!";
        }

        [HttpGet("{id}")]
        public TestClass GetWithId(Guid id) {
            return new TestClass();
        }

        [HttpPost]
        public void Create(TestClassWithFolder model) {
        }
    }
}
