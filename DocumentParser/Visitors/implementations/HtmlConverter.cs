using System;
using System.Collections.Generic;
using System.Text;
using Parser.Elements.Implementations;

namespace Parser.Visitors.implementations
{
    public class HtmlConverter : IDocumentVisitor
    {
        public const string LINE_BREAK = "\n";
        public const string DOUBLE_QUOTES = "\"";


        private readonly List<FootNoteElement> FootNoteElements = new List<FootNoteElement>();

        public string Visit(DocsElement element)
        {
            if (string.IsNullOrEmpty(element.ToString()))
            {
                return "";
            }
            return "<p>" + element.Value + "</p>";
        }

        public string Visit(AnchorElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<a href=").Append(DOUBLE_QUOTES).Append(element.Href).Append(DOUBLE_QUOTES)
                    .Append(">");
            builder.Append(element.Href);
            builder.Append("</a>");

            return builder.ToString();
        }

        public string Visit(AttributeElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<div class=" + DOUBLE_QUOTES + "attribution\">").Append(LINE_BREAK);
            builder.Append("&#8212; ").Append(element.By).Append("<br>").Append(LINE_BREAK);
            builder.Append("<cite>").Append(element.From).Append("</cite>").Append(LINE_BREAK);
            builder.Append("</div>").Append(LINE_BREAK);
            return builder.ToString();
        }

        public string Visit(BoldTextElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<strong>").Append(element.BoldText).Append("</strong>");
            return builder.ToString();
        }

        public string Visit(CommentElement element)
        {
            return "<!--" + element.Comment + "-->";
        }

        public string Visit(CrossReferenceElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<a href=").Append(DOUBLE_QUOTES).Append("#").Append(element.RefTarget).Append(DOUBLE_QUOTES)
                    .Append(">");
            builder.Append(element.AltText);
            builder.Append("</a>");

            return builder.ToString();
        }


        public string Visit(ExampleBlockElement element)
        {
            return "<p>" + element + "</p>";
        }

        public string Visit(FootNoteElement element)
        {
            FootNoteElements.Add(element);
            StringBuilder builder = new StringBuilder();


            String footnoteId = "_footnoteref_" + FootNoteElements.Count;

            builder.Append("<sup class=").Append(DOUBLE_QUOTES).Append("footnote").Append(DOUBLE_QUOTES).Append(">")
                    .Append("[")
                    .Append("<a ")
                    .Append("id=").Append(DOUBLE_QUOTES).Append(footnoteId).Append(DOUBLE_QUOTES)
                    .Append("class=").Append(DOUBLE_QUOTES).Append("footnote").Append(DOUBLE_QUOTES)
                    .Append("href=").Append(DOUBLE_QUOTES).Append("#").Append(footnoteId).Append(DOUBLE_QUOTES)
                    .Append(">")
                    .Append(FootNoteElements.Count)
                    .Append("</a>")
                    .Append("]")
                    .Append("</sup>");

            return builder.ToString();
        }

        public string Visit(HeadingElement element)
        {
            StringBuilder builder = new StringBuilder();
            String tag = "h" + element.Level;
            String id = element.Heading.Replace(" ", "_").ToLower();
            builder.Append("<").Append(tag)
                    .Append(" id=").Append(DOUBLE_QUOTES).Append(id).Append(DOUBLE_QUOTES).Append(">")
                    .Append(element.Heading)
                    .Append("</").Append(tag).Append(">");

            return builder.ToString();
        }

        public string Visit(ImageElement element)
        {
            return "<img class=image src=" + DOUBLE_QUOTES + element + DOUBLE_QUOTES + " alt=" +
                    DOUBLE_QUOTES + element.AltText + DOUBLE_QUOTES + "/>";
        }

        public string Visit(ItalicTextElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<em>").Append(element).Append("</em>");
            return builder.ToString();
        }

        public string Visit(ListingBlockElement element)
        {
            throw new System.NotImplementedException();
        }

        public string Visit(OrderedListElement element)
        {
            return "<ol>" + element + "</ol>";
        }

        public string Visit(QuotationElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<blockquote>").Append(LINE_BREAK);
            builder.Append("<div class=" + DOUBLE_QUOTES + "paragraph\">").Append(LINE_BREAK);
            builder.Append("<p>").Append(element).Append("</p>").Append(LINE_BREAK);
            builder.Append("</div>").Append(LINE_BREAK);
            builder.Append("</blockquote>").Append(LINE_BREAK);
            builder.Append(Visit(element.AttributeElement));
            return builder.ToString();
        }

        public string Visit(SideBarElement element)
        {
            return "<div class=" + DOUBLE_QUOTES + "side-bar\">" + element + "</div>";
        }

        public string Visit(TableElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table>").Append(LINE_BREAK);
            {
                builder.Append("<thead>").Append(LINE_BREAK);
                builder.Append("<tr>").Append(LINE_BREAK);

                
                foreach (String columnHeading in element.ColumnHeading)
                {
                    builder.Append("<th>").Append(columnHeading).Append("</th>").Append(LINE_BREAK);
                }
                builder.Append("</tr>").Append(LINE_BREAK);
                builder.Append("</thead>").Append(LINE_BREAK);
            }
            {
                builder.Append("<tbody>").Append(LINE_BREAK);
                foreach (String[] row in element.Rows)
                {
                    builder.Append("<tr>").Append(LINE_BREAK);
                    foreach (String column in row)
                    {
                        builder.Append("<td>").Append(column).Append("</td>").Append(LINE_BREAK);
                    }
                    builder.Append("</tr>").Append(LINE_BREAK);
                }
                builder.Append("</tbody>").Append(LINE_BREAK);
            }

            builder.Append("</table>").Append(LINE_BREAK);

            return builder.ToString();
        }

        public string Visit(TitleElement element)
        {
            return "<div class=" + DOUBLE_QUOTES + "title\">" + element + "</div>";
        }

        public string Visit(UnOrderedListElement element)
        {
            return "<ul>" + element + "</ul>";
        }
    }
}