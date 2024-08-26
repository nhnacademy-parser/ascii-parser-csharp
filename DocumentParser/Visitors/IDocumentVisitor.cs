using DocumentParser.Elements;

namespace DocumentParser.Visitors
{
    public interface IDocumentVisitor
    {
        string VisitDocument(IDocumentElement document);
    }
}