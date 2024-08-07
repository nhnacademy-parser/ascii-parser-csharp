using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ExampleBlockElement : DocsElement
    {
        public ExampleBlockElement(string example) : base(example)
        {
        }

        public string Example {
            get { return Value.ToString(); }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}