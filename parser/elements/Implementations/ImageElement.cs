namespace Parser.Elements.Implementations
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

        public string GetHref()
        {
            return Value.ToString();
        }
    }
}