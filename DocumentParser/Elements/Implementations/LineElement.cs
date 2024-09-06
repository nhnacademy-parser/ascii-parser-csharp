using System;
using DocumentParser.Domains.Nodes;
using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class LineElement : Node<string>, IDocumentElement
    {
        public LineElement()
        {
        }

        public LineElement(string value) : base(value)
        {
        }

        public virtual string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public ITree<IDocumentElement> Parent { get; set; }
    }
}