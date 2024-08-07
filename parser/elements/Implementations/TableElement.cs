using System.Collections.Generic;

namespace Parser.Elements.Implementations
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
    }
}