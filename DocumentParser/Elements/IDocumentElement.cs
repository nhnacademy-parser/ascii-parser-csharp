using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements
{
    public interface IDocumentElement
    {
        object Accept(IDocumentVisitor visitor);
        ITree<IDocumentElement> Parent { get; set; }
    }
}