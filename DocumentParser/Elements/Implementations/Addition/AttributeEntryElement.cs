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

          public string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
        public ITree<IDocumentElement> Parent { get; set; }
    }
}