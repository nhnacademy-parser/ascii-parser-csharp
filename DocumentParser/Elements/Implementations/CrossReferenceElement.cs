using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class CrossReferenceElement : DocsElement
    {
        public CrossReferenceElement(string refTarget) : base(refTarget)
        {
            AltText = "";
        }

        public CrossReferenceElement(string refTarget, string altText) : this(refTarget)
        {
            AltText = string.IsNullOrWhiteSpace(altText) ? refTarget : altText;
        }

        public string AltText { get; }

        public string RefTarget => Value.ToString();

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}