using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class HeadingElement : DocsElement
    {
        public HeadingElement(object heading) : base(heading)
        {
            Level = 1;
        }

        public HeadingElement(object heading, int level) : this(heading)
        {
            Level = level;
        }

        public int Level { get; }

        public string Heading => Value.ToString();
        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}