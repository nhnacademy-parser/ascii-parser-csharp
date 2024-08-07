using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
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

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}