using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class CommentElement : LineElement
    {
        public CommentElement()
        {
        }

        public CommentElement(string comment) : base(comment)
        {
        }


        public string Comment
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