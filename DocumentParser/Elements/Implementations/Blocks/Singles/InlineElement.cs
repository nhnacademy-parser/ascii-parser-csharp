using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks.Singles
{
    public class InlineElement : SingleContainerBlockElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
        
    }
}