using DocumentParser.Visitors;

namespace DocumentParser.Elements
{
    public abstract class IDocumentElement
    {
        public abstract object Accept(IDocumentVisitor visitor);
    }
}