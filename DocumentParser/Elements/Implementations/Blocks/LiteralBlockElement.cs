using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks
{
    public class LiteralBlockElement : BlockElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}