using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ListElement : IDocumentElement
    {
        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public IDocumentElement Append(IDocumentElement element)
        {
            throw new System.NotImplementedException();
        }
    }
}