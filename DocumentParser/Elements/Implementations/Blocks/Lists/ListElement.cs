using System.Collections.Generic;
using DocumentParser.Domains.Trees;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Blocks.Lists
{
    public class ListElement : SingleContainerBlockElement
    {
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
        
        
    }
}