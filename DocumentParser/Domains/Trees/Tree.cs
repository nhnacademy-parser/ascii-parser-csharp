using System.Collections.Generic;

namespace DocumentParser.Domain.Trees
{
    public interface ITree<T>
    {
        ICollection<T> Children { get; }
        ITree<T> Parent { get; }
    }
}