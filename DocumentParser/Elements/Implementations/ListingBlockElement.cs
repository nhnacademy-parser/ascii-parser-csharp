
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ListingBlockElement: DocsElement
    {
        public ListingBlockElement() : base()
        {
        }

        public string ListingBlock
        {
            get { return Value.ToString(); }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}