using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class BoldTextElement : DocsElement
    {
        public BoldTextElement(object boldText) : base(boldText)
        {
        }

        public string BoldText => Value.ToString();

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}