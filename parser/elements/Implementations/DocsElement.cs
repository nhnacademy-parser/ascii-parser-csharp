using System;
using Parser.Visitors;

namespace Parser.Elements.Implementations
{
    public class DocsElement:IElement
    {
      
        public DocsElement(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public virtual object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}