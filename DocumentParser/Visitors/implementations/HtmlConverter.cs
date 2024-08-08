using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Visitors.implementations
{
    public class HtmlConverter : IDocumentVisitor
    {
        public const char LINE_BREAK = '\n';
        public const char DOUBLE_QUOTES = '\"';


        private readonly List<FootNoteElement> FootNoteElements = new List<FootNoteElement>();

        public string Visit(DocsElement element)
        {
            StringBuilder stringBuilder = new StringBuilder();


            if (element != null)
            {
                if (element.Value != null)
                {
                    stringBuilder.Append(Visit(element.Value));
                }

                if (element.Children != null)
                {
                    foreach (DocsElement child in element.Children)
                    {
                        stringBuilder.Append(LINE_BREAK).Append(child.Accept(this));
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public string Visit(AnchorElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<a href=").Append(DOUBLE_QUOTES).Append(element.Href).Append(DOUBLE_QUOTES)
                    .Append(">");
            builder.Append(element.AltText);
            
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

            if (element.Next != null)
            {
                builder.Append(element.Next.Accept(this));
            }

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
            StringBuilder builder = new StringBuilder();

            builder.Append("<div>");

            foreach (DocsElement child in element.Children)
            {
                builder.Append("<p>");
                builder.Append(child.ValueString);
                builder.Append("</p>");

            }

            builder.Append("</div>");
            return builder.ToString();
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
            string tag = "h" + element.Level;
            string id = element.Heading.Replace(" ", "_").ToLower();
            builder.Append("<").Append(tag)
                    .Append(" id=").Append(DOUBLE_QUOTES).Append(id).Append(DOUBLE_QUOTES).Append(">")
                    .Append(element.Heading)
                    .Append("</").Append(tag).Append(">");

            return builder.ToString();
        }

        public string Visit(ImageElement element)
        {
            return "<img class=image src=" + DOUBLE_QUOTES + element.Href + DOUBLE_QUOTES + " alt=" +
                    DOUBLE_QUOTES + element.AltText + DOUBLE_QUOTES + "/>";
        }

        public string Visit(ItalicTextElement element)
        {

            StringBuilder builder = new StringBuilder();

            builder.Append("<em>").Append(element.ItalicText).Append("</em>");


            if (element.Next != null)
            {
                builder.Append(element.Next.Accept(this));
            }

            return builder.ToString();
        }

        public string Visit(IdAttributeElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<div id=").Append(DOUBLE_QUOTES).Append(element.ValueString).Append(DOUBLE_QUOTES).Append(">");
            builder.Append("</div>");
            return builder.ToString();
        }

        public string Visit(ListingBlockElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div>");

            foreach(DocsElement child in element.Children)
            {
                builder.Append("<pre>");

                builder.Append(child.ValueString);
                builder.Append("</pre>");

            }

            builder.Append("</div>");


            return builder.ToString();
        }

        public string Visit(LineElement element)
        {

            StringBuilder builder = new StringBuilder();

            builder.Append(element.Value);

            if(element.Next != null)
            {
                builder.Append(element.Next.Accept(this));
            }

            return builder.ToString();
        }

        public string Visit(OrderedListElement element)
        {

            StringBuilder builder = new StringBuilder();

            builder.Append("<ol>");

            builder.Append("<ul>").Append(LINE_BREAK);

            builder.Append("<li>").Append(LINE_BREAK);
            builder.Append(element.ValueString);
            foreach (DocsElement docs in element.Children)
            {
                builder.Append(docs.Accept(this));
            }
            builder.Append("</li>").Append(LINE_BREAK);

            builder.Append("</ol>").Append(LINE_BREAK);

            return builder.ToString();
        }

        public string Visit(QuotationElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<blockquote>").Append(LINE_BREAK);
            builder.Append("<div class=" + DOUBLE_QUOTES + "paragraph\">").Append(LINE_BREAK);
            builder.Append("<p>");
            foreach (DocsElement child in element.Children)
            {
                builder.Append(child.ValueString);
            }
            builder.Append("</p>").Append(LINE_BREAK);
            builder.Append("</div>").Append(LINE_BREAK);
            builder.Append("</blockquote>").Append(LINE_BREAK);
            //builder.Append(Visit(element.AttributeElement));
            return builder.ToString();
        }

        public string Visit(SideBarElement element)
        {

            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=").Append(DOUBLE_QUOTES).Append("side-bar").Append(DOUBLE_QUOTES).Append(">");

            foreach(DocsElement child in element.Children)
            {
                builder.Append("<pre>");
                builder.Append(child.ValueString);
                builder.Append("</pre>");

            }

            builder.Append("</div>");


            return builder.ToString();
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
            return "<div class=" + DOUBLE_QUOTES + "title\">" + element.Title + "</div>";
        }

        public string Visit(UnOrderedListElement element)
        {

            StringBuilder builder = new StringBuilder();

            builder.Append("<ul>").Append(LINE_BREAK);

            builder.Append("<li>").Append(LINE_BREAK);
            builder.Append(element.ValueString);
            foreach (DocsElement docs in element.Children)
            { 
                builder.Append(docs.Accept(this));
            }
            builder.Append("</li>").Append(LINE_BREAK);

            builder.Append("</ul>").Append(LINE_BREAK);

            return builder.ToString();
        }

        public string Visit(string s)
        {
            return "<p>" + s + "</p>";
        }

        public string Visit(object o)
        {
            if (o is DocsElement doc)
            {
                return doc.Accept(this).ToString();
            }

            return "<p>" + o.ToString() + "</p>";
        }
    }
}