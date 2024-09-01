using System;
using System.Collections.Generic;
using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class BlockElement : DocumentElement, ITree<IDocumentElement>
    {
        private ICollection<IDocumentElement> _children = new List<IDocumentElement>();

        public int Count => _children.Count;

        public ICollection<IDocumentElement> Children
        {
            get => _children;
            set
            {
                _children = value;
                foreach (IDocumentElement child in _children)
                {
                    child.Parent = this;
                }
            }
        }

        public void AddChild(IDocumentElement child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public virtual bool IsFulled()
        {
            return false;
        }
    }
}