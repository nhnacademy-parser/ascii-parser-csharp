using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class ImageReferenceElement : LineElement
    {
        public ImageReferenceElement()
        {
        }

        public ImageReferenceElement(string imageReference, string altText) : this(imageReference)
        {
            AltText = altText;
        }

        public ImageReferenceElement(string imageReference) : base(imageReference)
        {
            AltText = imageReference;
        }

        public string ImageReference
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