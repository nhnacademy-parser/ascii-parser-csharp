using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class AnchorElement : LineElement
    {
        public AnchorElement()
        {
        }

        public AnchorElement(string reference, string altText) : this(reference)
        {
            AltText = altText;
        }

        public AnchorElement(string reference) : base(reference)
        {
            AltText = reference;
        }

        public string Reference
        {
            get => Value;
            set => Value = value;
        }

        public string AltText { get; set; }
        
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}