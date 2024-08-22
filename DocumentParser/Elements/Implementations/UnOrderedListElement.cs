using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class UnOrderedListElement : DocsElement

    {
        public UnOrderedListElement(object value) : base(value)
        {
            Value = value;
            Level = 1;
            InitContainer();
        }

        public UnOrderedListElement(object value, int level) : this(value)
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