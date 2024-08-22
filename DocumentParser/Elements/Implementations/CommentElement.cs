using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class CommentElement:DocsElement
    {
        public CommentElement(string comment):base(comment)
        {
        }

        public string Comment => Value.ToString();

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}