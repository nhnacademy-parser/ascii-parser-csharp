using Parser.Visitors;

namespace Parser.Elements
{
    public interface IElement
    {
        object Accept(IDocumentVisitor visitor);
    }
}

