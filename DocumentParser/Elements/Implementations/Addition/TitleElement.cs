using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Addition
{
    public class TitleElement: IDocumentElement
    {
        public TitleElement(string title) 
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