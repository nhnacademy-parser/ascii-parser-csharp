using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class DocumentElement : IDocumentElement
    {
        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        IDocumentElement Title { get; set; }
        Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public void AddAttribute(string key, string value)
        {
            Attributes.Add(key, value);
        }
    }
}