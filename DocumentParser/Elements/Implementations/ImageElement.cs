using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ImageElement : DocsElement
    {

        public ImageElement(string href) : base(href)
        {
            AltText = "";
        }

        public ImageElement(string href, string altText) : this(href)
        {
            AltText = altText;
        }

        public string AltText { get; }

        public string Href
        {
            get { return Value.ToString(); }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}