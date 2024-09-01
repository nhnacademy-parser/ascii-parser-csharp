using System.Collections.Generic;
using DocumentParser.Domains.Trees;
using DocumentParser.Elements.Implementations.Inlines;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks.Lists
{
    public class ListElement : IDocumentElement
    {
        public ListElement(IDocumentElement value)
        {
            Value = value;
        }

        public ListElement(string paragraph)
        {
            Value = new ParagraphElement() { Paragraph = paragraph };
        }

        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public ITree<IDocumentElement> Parent { get; set; }

        IDocumentElement Value { get; set; }
    }
}