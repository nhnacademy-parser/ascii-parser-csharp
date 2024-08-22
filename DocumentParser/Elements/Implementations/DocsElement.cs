using System;
using System.Collections;
using DocumentParser.Visitors;
using System.Collections.Generic;
using System.Text;

namespace DocumentParser.Elements.Implementations
{
    public class DocsElement : IDocumentElement
    {
        private AttributeElement _attributeElement;

        public DocsElement()
        {
        }

        protected DocsElement(object boldText)
        {
            Value = boldText;
        }

        public object Value { get; protected set; }
        public IDocumentElement Next { get; private set; }
        public DocsElement Parent { get; set; }

        public virtual object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public void AddChild(DocsElement docsElement)
        {
            if (IsContainer)
            {
                Children.Add(docsElement);
            }
            else
            {
                throw new NotSupportedException("this Element can't have children");
            }
        }

        protected void InitContainer()
        {
            Children = new List<IDocumentElement>();
        }

        public bool IsContainer => Children != null;

        public List<IDocumentElement> Children { get; private set; }

        public IDocumentElement Append(IDocumentElement element)
        {
            return Next = element;
        }

        public void AddAttribute(AttributeElement attributeElement)
        {
            _attributeElement = attributeElement;
        }
    }
}