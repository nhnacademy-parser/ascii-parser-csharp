using System;
using DocumentParser.Elements.Implementations;
using DocumentParser.Visitors.implementations;

namespace DocumentParser.Domain
{
    public class Document
    {

        internal void AddRootElement(DocsElement headElement)
        {
            RootElement = headElement;
        }

        public DocsElement RootElement { get; set; }

        public override string ToString()
        {
            return RootElement.ToString();
        }

        public string convert(HtmlConverter htmlConverter)
        {
            return RootElement.Accept(htmlConverter).ToString();
        }
    }
}

