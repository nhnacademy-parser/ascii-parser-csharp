using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class FootNoteElement : LineElement
    {
        public FootNoteElement()
        {
        }

        public string Footnote
        {
            get => Value;
            set => Value = value;
        }

        public FootNoteElement(string footnote) : base(footnote)
        {
            Footnote = footnote;
        }
        
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}