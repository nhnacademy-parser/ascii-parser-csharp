using System.Collections.Generic;
using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class DocumentElement : IDocumentElement
    {
        public virtual string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public void AddAttribute(string key, string value)
        {
            Attributes.Add(key, value);
        }

        public ITree<IDocumentElement> Parent { get; set; }
    }
}