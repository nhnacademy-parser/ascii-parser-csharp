using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class SectionTitleElement : IDocumentElement
    {
        public SectionTitleElement(string title)
        {
            Title = title;
        }

        public string Title { get; set; }

        public override object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}