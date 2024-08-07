namespace Parser.Elements.Implementations
{
    public class CommentElement:DocsElement
    {
        public CommentElement(string comment):base(comment)
        {
        }

        public string Comment
        {
            get { return Value.ToString(); }
        }
    }
}