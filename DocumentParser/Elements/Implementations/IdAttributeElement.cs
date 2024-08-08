using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class IdAttributeElement : DocsElement
    {
        public IdAttributeElement(object value) : base(value)
        {
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}