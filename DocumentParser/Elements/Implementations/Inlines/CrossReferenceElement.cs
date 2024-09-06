using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations.Inlines
{
    public class CrossReferenceElement : LineElement
    {
        public CrossReferenceElement()
        {
        }

        public CrossReferenceElement(string crossRef, string altText) : this(crossRef)
        {
            AltText = altText;
        }

        public CrossReferenceElement(string crossRef) : base(crossRef)
        {
            AltText = crossRef;
        }

        public string CrossReference
        {
            get => Value;
            set => Value = value;
        }

        public string AltText { get; set; }
        
        public override string Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}