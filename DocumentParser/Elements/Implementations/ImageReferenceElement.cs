using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ImageReferenceElement : DocsElement
    {

        public ImageReferenceElement(string href) : base(href)
        {
            AltText = "";
        }

        public ImageReferenceElement(string href, string altText) : this(href)
        {
            AltText = altText;
        }

        public string AltText { get; }

        public string Href => Value.ToString();

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}