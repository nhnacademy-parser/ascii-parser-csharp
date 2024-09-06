using System.Collections.Generic;
using DocumentParser.Elements;

namespace DocumentParser.Domains.Trees
{
    public interface ITree<T>
    {
        IList<IDocumentElement> Children { get; }
        
        ITree<T> Parent { get; }

        void AddChild(IDocumentElement child);
    }
}