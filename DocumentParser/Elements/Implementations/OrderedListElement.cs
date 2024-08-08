using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class OrderedListElement : DocsElement
    {
        public OrderedListElement(object value) : base()
        {
            Value = value;
            Level = 1;
        }

        public OrderedListElement(object value, int level) : this(value)
        {
            Level = level;
        }

        public int Level { get; }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override bool EndOfContaier(DocsElement element)
        {
            if (element is OrderedListElement ol)
            {
                return ol.Level <= Level;
            }
            else
            {
                return string.IsNullOrWhiteSpace(element.Value.ToString());
            }
        }
    }
}