namespace Parser.Elements.Implementations
{
    public class FootNoteElement : DocsElement
    {
        public FootNoteElement(string example) : base(example)
        {
        }

        public string FootNote
        {
            get { return Value.ToString(); }
        }
    }
}