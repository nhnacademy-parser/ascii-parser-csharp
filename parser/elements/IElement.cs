using parser.visitors;

namespace parser.elements
{
    public interface IElement
    {
        void Accept(IDocumentVisitor visitor);
    }
}

