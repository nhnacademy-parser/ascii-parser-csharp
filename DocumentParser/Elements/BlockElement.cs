using System;
using System.Collections;
using System.Collections.Generic;
using DocumentParser.Domain.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements
{
    public class BlockElement : IDocumentElement, ITree<IDocumentElement>
    {
        private ICollection<IDocumentElement> _children = new List<IDocumentElement>();
        private ITree<IDocumentElement> _parent;

        public int Count => _children.Count;

        public override object Accept(IDocumentVisitor visitor)
        {
            throw new NotImplementedException();
        }

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