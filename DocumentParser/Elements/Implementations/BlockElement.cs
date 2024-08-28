using System;
using System.Collections.Generic;
using DocumentParser.Domain.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class BlockElement : DocumentElement, ITree<IDocumentElement>
    {
        private ICollection<IDocumentElement> _children = new List<IDocumentElement>();
        private ITree<IDocumentElement> _parent;

        public int Count => _children.Count;

        public ICollection<IDocumentElement> Children
        {
            get => _children;
            set => _children = value;
        }

        public ITree<IDocumentElement> Parent
        {
            get => _parent;
            set => _parent = value;
        }
    }
}