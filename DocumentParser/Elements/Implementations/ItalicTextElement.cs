using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ItalicTextElement : DocsElement
    {
        public ItalicTextElement(string italicText) : base(italicText)
        {
        }

        public string ItalicText
        {
            get { return Value.ToString(); }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}