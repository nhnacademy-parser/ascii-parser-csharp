using System;
using DocumentParser.Domains.Nodes;
using DocumentParser.Visitors;

namespace DocumentParser.Elements
{
    public class LineElement : Node<IDocumentElement>
    {
        public object Accept(IDocumentVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}