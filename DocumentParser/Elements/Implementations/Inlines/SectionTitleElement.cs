using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class SectionTitleElement : IDocumentElement
    {
        public SectionTitleElement()
        {
        }

        public string Title { get; set; }
        public int Level { get; set; }
        
        public ITree<IDocumentElement> Parent { get; set; }
        
        public string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}