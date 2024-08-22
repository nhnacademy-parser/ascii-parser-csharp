﻿
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ListingBlockElement: DocsElement
    {
        public ListingBlockElement(object value) : base(value)
        {
            InitContainer();
        }
        
        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}