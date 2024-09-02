using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentParser.Domains.Trees;

namespace DocumentParser.Elements.Implementations
{
    public class BlockElement : DocumentElement, ITree<IDocumentElement>
    {
        private IList<IDocumentElement> _children = new List<IDocumentElement>();

        public int Count => _children.Count;

        public IList<IDocumentElement> Children
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

        public virtual void AddChild(IDocumentElement child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public virtual bool IsFulled()
        {
            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.GetType().Name).Append(" { ");
            builder.Append(string.Join(",", _children.Select(x => x.ToString())));
            builder.Append("}");

            return builder.ToString();
        }
    }
}