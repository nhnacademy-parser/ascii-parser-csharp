using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class ParagraphElement : IDocumentElement
    {
        public ParagraphElement(string paragraph)
        {
            Paragraph = paragraph;
        }

        public string Paragraph { get; set; }

        public override object Accept(IDocumentVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}