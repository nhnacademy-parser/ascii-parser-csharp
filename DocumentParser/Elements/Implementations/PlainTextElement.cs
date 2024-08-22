using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class PlainTextElement : DocsElement
    {
        public PlainTextElement(string value) : base(value)
        {
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}