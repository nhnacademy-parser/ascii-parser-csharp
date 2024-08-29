using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class SectionTitleElement : IDocumentElement
    {
        public SectionTitleElement(string title)
        {
            Title = title;
        }

        public string Title { get; set; }

        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public ITree<IDocumentElement> Parent { get; set; }
    }
}