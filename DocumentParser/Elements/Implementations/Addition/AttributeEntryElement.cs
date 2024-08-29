using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Addition
{
    public class AttributeEntryElement : IDocumentElement
    {
        public AttributeEntryElement(string attribute)
        {
        }

        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public ITree<IDocumentElement> Parent { get; set; }
    }
}