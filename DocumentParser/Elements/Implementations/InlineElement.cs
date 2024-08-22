using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class InlineElement : DocsElement
    {
        public InlineElement(object value) : base(value)
        {
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}