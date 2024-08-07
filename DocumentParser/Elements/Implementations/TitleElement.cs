namespace Parser.Elements.Implementations
{
    public class TitleElement : DocsElement
    {
        public TitleElement(string title) : base(title)
        {
        }

        public string Title { get { return Value.ToString(); } }
    }
}