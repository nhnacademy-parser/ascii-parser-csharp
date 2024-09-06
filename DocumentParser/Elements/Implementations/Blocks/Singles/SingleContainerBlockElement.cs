using System;

namespace DocumentParser.Elements.Implementations.Blocks.Singles
{
    public class SingleContainerBlockElement : BlockElement
    {
        public new void AddChild(IDocumentElement child)
        {
            if (!IsFulled())
            {
                base.AddChild(child);
                child.Parent = this;
            }
            else
            {
                throw new InvalidOperationException("Cannot add child to container, Maybe Container is full.");
            }
        }

        public override bool IsFulled()
        {
            return base.Count > 0;
        }

        public IDocumentElement Value => Children[0];
    }
}