using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ItalicTextElement : DocsElement
    {
        public ItalicTextElement(object italicText) : base(italicText)
        {
        }

        public string ItalicText => Value.ToString();
        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}