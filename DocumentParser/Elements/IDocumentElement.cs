using DocumentParser.Visitors;

namespace DocumentParser.Elements
{
    public interface IDocumentElement
    {
        object Accept(IDocumentVisitor visitor);
    }
}