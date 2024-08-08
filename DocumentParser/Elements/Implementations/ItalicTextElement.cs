using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ItalicTextElement : DocsElement
    {
        public ItalicTextElement(object italicText) : base(italicText)
        {
        }

        public string ItalicText
        {
            get { return ValueString; }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}