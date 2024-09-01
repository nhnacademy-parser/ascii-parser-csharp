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

            throw new InvalidOperationException("Cannot add child to container, Maybe Container is full.");
        }

        public override bool IsFulled()
        {
            return base.Count > 0;
        }
    }
}