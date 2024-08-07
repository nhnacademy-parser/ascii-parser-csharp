using System.Collections.Generic;

namespace Parser.Elements.Implementations
{
    public class OrderedListElement : DocsElement
    {
        public OrderedListElement(List<DocsElement> value) : base(value)
        {
            Level = 1;
        }

        public OrderedListElement(List<DocsElement> value, int level) : base(value)
        {
            Level = level;
        }

        public int Level { get; }
    }
}