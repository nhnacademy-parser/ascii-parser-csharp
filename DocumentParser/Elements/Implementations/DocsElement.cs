using System;
using DocumentParser.Visitors;
using System.Collections.Generic;
using System.Text;

namespace DocumentParser.Elements.Implementations
{
    public class DocsElement:IElement
    {
        public DocsElement()
        {
            Children = new List<DocsElement>();
        }
      
        public DocsElement(object value)
        {
            Value = value;
        }

        public object Value {get;  protected set; }
        public string ValueString { get { return Value.ToString().TrimEnd(); } }
        public DocsElement Next { get; set; }
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
            if(IsContainer)
            {
                Children.Add(docsElement);
            }
            else
            {
                throw new NotSupportedException("this Element can't have children");
            }

        }

        public virtual bool EndOfContaier(DocsElement element)
        {
            if (element.IsContainer)
            {
                return Type.Equals(this.GetType(), element.GetType());
            }

            return false;
        }

        public bool IsContainer
        {
            get { return Children != null; }
        }

        public List<DocsElement> Children { get; }

        public DocsElement Append(DocsElement element)
        { 
            return Next = element;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            DocsElement temp = this;
            for(; temp.Parent != null; temp = temp.Parent)
            {
                stringBuilder.Append(' ');
            }

            stringBuilder.Append(Value).Append('\n');

            if(Children == null)
            {
                return stringBuilder.ToString();
            }
            foreach(DocsElement child in Children)
            {
                stringBuilder.Append(child.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}