namespace Parser.Elements.Implementations
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
    }
}