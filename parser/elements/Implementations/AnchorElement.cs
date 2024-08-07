namespace Parser.Elements.Implementations
{
    public class AnchorElement : DocsElement
    {

        public AnchorElement(string href): base(href)
        {
            AltText = "";
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
        
    }
}