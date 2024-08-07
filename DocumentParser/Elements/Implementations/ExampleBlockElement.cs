namespace Parser.Elements.Implementations
{
    public class ExampleBlockElement : DocsElement
    {
        public ExampleBlockElement(string example) : base(example)
        {
        }

        public string Example {
            get { return Value.ToString(); }
        }
    }
}