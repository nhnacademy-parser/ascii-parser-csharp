using System.Collections.Generic;
using DocumentParser.Domains.Nodes;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.Elements.Implementations.Blocks.Lists
{
    public class ListContainerElement : BlockElement
    {
        public override void AddChild(IDocumentElement child)
        {
            ListElement listElement = new ListElement();
            listElement.Children.Add(child);
            
            child.Parent = this;
            
            AddChild(listElement);
        }

        public void AddChild(ListElement child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void AddChild(string child)
        {
            AddChild(new ParagraphElement() { Paragraph = child });
        }
    }
}