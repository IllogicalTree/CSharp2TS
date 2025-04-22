using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public record TSType(TypeReference Type, string TSTypeShortName, string TSTypeFullName, bool IsObject);
}
