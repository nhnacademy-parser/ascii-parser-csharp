using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
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

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}