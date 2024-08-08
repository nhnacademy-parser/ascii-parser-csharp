using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class LineElement : DocsElement
    {
        public LineElement(object value) : base(value)
        {
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}