using System.Collections;
using System.Collections.Generic;
using System.Text;
using DocumentParser.Elements;
using DocumentParser.Visitors.implementations;

namespace DocumentParser.Domain
{
    public class Document : IEnumerable<IDocumentElement>
    {


        private List<IDocumentElement> _elements;

        public Document()
        {
            _elements = new List<IDocumentElement>();
        }

        public object Header { get; set; }
        public List<IDocumentElement> Body { get; set; }
        public object Footer { get; set; }

        public void Append(IDocumentElement element)
        {
            _elements.Add(element);
        }

        public IEnumerator<IDocumentElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

