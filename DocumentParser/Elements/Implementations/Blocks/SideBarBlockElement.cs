using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks
{
    public class SideBarBlockElement : BlockElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}