using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Addition
{
    public class AttributeEntryElement : IDocumentElement
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public AttributeEntryElement()
        {
        }

        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public ITree<IDocumentElement> Parent { get; set; }
    }
}