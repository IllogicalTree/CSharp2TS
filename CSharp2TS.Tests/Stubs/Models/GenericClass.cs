#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.Tests.Stubs.Models {
    [TSInterface]
    public class GenericClass1<T> {
        public T Value { get; set; }
        public IList<T> Values { get; set; }
    }

    [TSInterface]
    public class GenericClass2<T1, T2> {
        public T1 Value1 { get; set; }
        public IList<T1> Values1 { get; set; }
        public T2 Value2 { get; set; }
        public IList<T2> Values2 { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
