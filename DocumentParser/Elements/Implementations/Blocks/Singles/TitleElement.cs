using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks.Singles
{
    public class TitleElement: SingleContainerBlockElement
    {
        public string Title { get; set; }
        
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}