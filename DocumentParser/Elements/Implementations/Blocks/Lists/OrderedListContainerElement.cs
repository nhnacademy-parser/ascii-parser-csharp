using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks.Lists
{
    public class OrderedListContainerElement : ListContainerElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}