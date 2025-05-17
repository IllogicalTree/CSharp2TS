using CSharp2TS.Core.Attributes;
using System.Text.Json;

namespace CSharp2TS.Tests.Stubs.Models {
    [TSInterface]
    public class TestJsonElement {
        public JsonElement json { get; set; }
    }
}
