using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class TableElement : DocsElement
    {

        public TableElement(string[] columnHeading, List<string[]> rows, TitleElement titleElement) : base(titleElement)
        {
            ColumnHeading = columnHeading;
            Rows = rows;
        }

        public string[] ColumnHeading { get; }
        public List<string[]> Rows { get; }
        public string Title { get { return Value.ToString(); } }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}