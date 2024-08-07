namespace Parser.Elements.Implementations
{
    public class CrossReferenceElement : DocsElement
    {
        public CrossReferenceElement(string refTarget) : base(refTarget)
        {
            AltText = "";
        }

        public CrossReferenceElement(string refTarget, string altText) : this(refTarget)
        {
            AltText = altText;
        }

        public string AltText { get; }

        public string RefTarget
        {
            get { return Value.ToString(); }
        }
    }
}