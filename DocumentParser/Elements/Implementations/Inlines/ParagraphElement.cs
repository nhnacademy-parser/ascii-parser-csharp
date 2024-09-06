using DocumentParser.Domains.Trees;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class ParagraphElement : LineElement
    {
        public ParagraphElement()
        {
        }

        public ParagraphElement(string paragraph) : base(paragraph)
        {
        }

        public string Paragraph
        {
            get => Value;
            set => Value = value;
        }


        public override string ToString()
        {
            return "paragraph {" + Paragraph + "}";
        }
        
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}