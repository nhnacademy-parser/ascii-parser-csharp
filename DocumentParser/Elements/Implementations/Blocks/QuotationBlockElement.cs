using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks
{
    public class QuotationBlockElement : BlockElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}