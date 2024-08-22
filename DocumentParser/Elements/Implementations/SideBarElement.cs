using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class SideBarElement : DocsElement
    {
        public SideBarElement(object value) : base(value)
        {
            Value = value;
            InitContainer();
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}