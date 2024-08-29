using System.Collections.Generic;
using DocumentParser.Elements;

namespace DocumentParser.Domains.Trees
{
    public interface ITree<T>
    {
        ICollection<T> Children { get; }
        
        ITree<T> Parent { get; }

        void AddChild(IDocumentElement child);
    }
}