using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class FootNoteElement : DocsElement
    {
        public FootNoteElement(object footNote) : base(footNote)
        {
        }

        public string FootNote => Value.ToString();

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}