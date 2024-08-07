using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
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

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}