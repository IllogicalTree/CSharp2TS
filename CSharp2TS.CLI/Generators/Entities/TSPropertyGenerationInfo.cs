using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public record TSPropertyGenerationInfo(TypeReference Type, string TSType, string TSTypeFull, bool IsObject);
}
