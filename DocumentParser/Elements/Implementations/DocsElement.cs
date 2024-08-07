using System;
using DocumentParser.Visitors;
using System.Collections.Generic;

namespace DocumentParser.Elements.Implementations
{
    public class DocsElement:IElement
    {
        public DocsElement()
        {
            Value = new List<DocsElement>();
        }
      
        public DocsElement(object value)
        {
            Value = value;
        }

        public object Value { get; }
        public DocsElement Parent
        {
            get; set;
        }

        public virtual object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public void AddChild(DocsElement docsElement)
        {
            if(Value is List<DocsElement> children)
            {
                children.Add(docsElement);
            }

            throw new NotSupportedException("this Element can't have children");
        }

        public bool EndOfContaier(DocsElement element)
        {
            throw new NotImplementedException();
        }

        public bool IsContainer
        {
            get { return Value is List<DocsElement>; }
        }


    }
}