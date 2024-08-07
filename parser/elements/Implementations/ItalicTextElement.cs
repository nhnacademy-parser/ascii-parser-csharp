namespace Parser.Elements.Implementations
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
    }
}