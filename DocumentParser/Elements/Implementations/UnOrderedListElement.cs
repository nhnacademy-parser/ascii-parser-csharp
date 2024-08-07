using System.Collections.Generic;

namespace Parser.Elements.Implementations
{
    public class UnOrderedListElement:DocsElement

    {
        public UnOrderedListElement(List<DocsElement> value) : base(value)
        {
            Level = 1;
        }

        public UnOrderedListElement(List<DocsElement> value, int level) : base(value)
        {
            Level = level;
        }

        public int Level { get; }
    }
}