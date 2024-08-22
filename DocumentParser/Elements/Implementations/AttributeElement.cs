using System.Collections;
using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class AttributeElement : DocsElement, IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public void AddAttribute(string key, string value)
        {
            _attributes.Add(key, value);
        }


        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}