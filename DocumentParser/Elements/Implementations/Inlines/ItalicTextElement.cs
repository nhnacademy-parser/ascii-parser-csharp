using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class ItalicTextElement : LineElement
    {
        public ItalicTextElement()
        {
        }

        public ItalicTextElement(string italicText) : base(italicText)
        {
        }

        public string ItalicText
        {
            get => Value;
            set => Value = value;
        }
        
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}