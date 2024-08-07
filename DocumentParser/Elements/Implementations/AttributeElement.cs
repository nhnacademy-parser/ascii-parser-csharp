namespace Parser.Elements.Implementations
{
    public class AttributeElement : DocsElement
    {
        public AttributeElement(string by) : base(by)
        {
            QutationType = "quote";
            From = "";
        }

        public AttributeElement(string quatationType, string by, string from) : this(by)
        {
            QutationType = quatationType;
            From = from;
        }

        public string QutationType { get; }
        public string From { get; }
        public string By
        {
            get { return Value.ToString();  }
        }
    }
}