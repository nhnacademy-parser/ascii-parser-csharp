using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class TitleElement : DocsElement
    {
        public TitleElement(string title) : base(title)
        {
        }

        public string Title { get { return Value.ToString(); } }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}