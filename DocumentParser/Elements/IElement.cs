using DocumentParser.Visitors;

namespace DocumentParser.Elements
{
    public interface IElement
    {
        object Accept(IDocumentVisitor visitor);
    }
}

