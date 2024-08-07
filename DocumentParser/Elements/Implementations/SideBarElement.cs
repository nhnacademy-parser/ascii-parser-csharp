﻿using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class SideBarElement : DocsElement
    {
        public SideBarElement(string sideBar) : base(sideBar)
        {
           
        }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}