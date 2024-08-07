using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class AnchorElement : DocsElement
    {

        public AnchorElement(string href): base(href)
        {
            AltText = href;
        }

        public AnchorElement(string href, string altText) : this(href)
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