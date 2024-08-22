using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class AnchorElement : DocsElement
    {
        public AnchorElement(string href) : base(href)
        {
            AltText = href;
        }

        public AnchorElement(string href, string altText) : this(href)
        {
            AltText = string.IsNullOrWhiteSpace(altText) ? href : altText;
        }

        public string AltText { get; }

        public string Href => Value.ToString();


        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}