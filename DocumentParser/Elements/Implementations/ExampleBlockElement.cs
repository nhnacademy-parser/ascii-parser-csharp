using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ExampleBlockElement : DocsElement
    {
        public ExampleBlockElement(string value) : base(value)
        {
            InitContainer();
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}