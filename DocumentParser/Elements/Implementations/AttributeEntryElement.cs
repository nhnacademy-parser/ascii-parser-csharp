using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class AttributeEntryElement : IDocumentElement
    {
        public AttributeEntryElement(string attribute)
        {
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}