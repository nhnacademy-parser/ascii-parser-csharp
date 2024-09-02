using System.Collections.Generic;
using DocumentParser.Domains.Trees;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks.Lists
{
    public class ListElement : SingleContainerBlockElement
    {
        public object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public IDocumentElement Value => Children[0];
    }
}