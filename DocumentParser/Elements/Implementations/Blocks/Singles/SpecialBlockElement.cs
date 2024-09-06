using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks
{
    public class SpecialBlockElement : SingleContainerBlockElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}