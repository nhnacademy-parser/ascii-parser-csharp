using Parser.Visitors;

namespace Parser.Elements.Implementations
{
    public class HeadingElement : DocsElement
    {
        public HeadingElement(string heading) : base(heading)
        {
            Level = 1;
        }

        public HeadingElement(string heading, int level) : this(heading)
        {
            Level = level;
        }

        public int Level { get; }
        public string Heading
        {
            get { return Value.ToString(); }
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}