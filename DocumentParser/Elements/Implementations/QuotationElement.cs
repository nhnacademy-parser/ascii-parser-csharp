using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class QuotationElement : DocsElement
    {
        public QuotationElement(object quotation) : base(quotation)
        {
            InitContainer();
        }

        public AttributeElement AttributeElement
        {
            get; set;
        }
        
        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}