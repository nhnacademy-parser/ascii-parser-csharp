using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class BoldTextElement : LineElement
    {
        public BoldTextElement()
        {
        }

        public BoldTextElement(string boldText) : base(boldText)
        {
            BoldText = boldText;
        }


        public string BoldText
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