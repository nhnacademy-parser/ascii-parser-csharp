using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class BoldTextElement : DocsElement
    {
        public BoldTextElement(string boldText) : base(boldText)
        {
        }

        public string BoldText
        {
            get { return Value.ToString(); }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}