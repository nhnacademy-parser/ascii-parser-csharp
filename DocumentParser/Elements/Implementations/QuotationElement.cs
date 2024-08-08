﻿using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class QuotationElement : DocsElement
    {
        public QuotationElement(object quotation) : base()
        {
        }

        public AttributeElement AttributeElement
        {
            get; set;
        }
        
        public string Quotation { get { return Value.ToString(); } }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}