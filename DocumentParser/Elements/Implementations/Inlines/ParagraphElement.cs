using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class ParagraphElement : IDocumentElement
    {
        public ParagraphElement()
        {
        }

        public string Paragraph { get; set; }

        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public ITree<IDocumentElement> Parent { get; set; }

        public override string ToString()
        {
            return "paragraph {" + Paragraph + "}";
            
        }
    }
}