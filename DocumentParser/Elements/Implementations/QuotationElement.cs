namespace Parser.Elements.Implementations
{
    public class QuotationElement : DocsElement
    {
        public QuotationElement(string quotation) : base(quotation)
        {
        }

        public AttributeElement AttributeElement
        {
            get; set;
        }
        
        public string Quotation { get { return Value.ToString(); } }
    }
}