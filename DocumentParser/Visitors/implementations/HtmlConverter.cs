using System;
using System.Collections.Generic;
using System.Text;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Visitors.implementations
{
    public class HtmlConverter : IDocumentVisitor
    {
        private const char LineBreak = '\n';
        private const char DoubleQuotes = '\"';


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
                        stringBuilder.Append(LineBreak).Append(child.Accept(this));
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public string Visit(AnchorElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<a href=").Append(DoubleQuotes).Append(element.Href).Append(DoubleQuotes)
                .Append(">");
            builder.Append(element.AltText);

            builder.Append("</a>");

            return builder.ToString();
        }

        public string Visit(AttributeElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" ");
            foreach (KeyValuePair<string, string> keyValuePair in element)
            {
                builder.Append(keyValuePair.Key).Append("=").Append(keyValuePair.Value).Append(" ");
            }

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

            builder.Append("<a href=").Append(DoubleQuotes).Append("#").Append(element.RefTarget).Append(DoubleQuotes)
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
                builder.Append(child.Value);
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

            builder.Append("<sup class=").Append(DoubleQuotes).Append("footnote").Append(DoubleQuotes).Append(">")
                .Append("[")
                .Append("<a ")
                .Append("id=").Append(DoubleQuotes).Append(footnoteId).Append(DoubleQuotes)
                .Append("class=").Append(DoubleQuotes).Append("footnote").Append(DoubleQuotes)
                .Append("href=").Append(DoubleQuotes).Append("#").Append(footnoteId).Append(DoubleQuotes)
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
                .Append(" id=").Append(DoubleQuotes).Append(id).Append(DoubleQuotes).Append(">")
                .Append(element.Heading)
                .Append("</").Append(tag).Append(">");

            return builder.ToString();
        }

        public string Visit(ImageElement element)
        {
            return "<img class=image src=" + DoubleQuotes + element.Href + DoubleQuotes + " alt=" +
                   DoubleQuotes + element.AltText + DoubleQuotes + "/>";
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

        public string Visit(ListingBlockElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div>");

            foreach (DocsElement child in element.Children)
            {
                builder.Append("<pre>");

                builder.Append(child.Value);
                builder.Append("</pre>");
            }

            builder.Append("</div>");


            return builder.ToString();
        }

        public string Visit(InlineElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<p>");

            if (element.Value is DocsElement child)
            {
                builder.Append(child.Accept(this));
            }

            builder.Append("</p>");

            return builder.ToString();
        }

        public string Visit(OrderedListElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<ol>");

            builder.Append("<ul>").Append(LineBreak);

            builder.Append("<li>").Append(LineBreak);
            builder.Append(element.Value);
            foreach (DocsElement docs in element.Children)
            {
                builder.Append(docs.Accept(this));
            }

            builder.Append("</li>").Append(LineBreak);

            builder.Append("</ol>").Append(LineBreak);

            return builder.ToString();
        }

        public string Visit(QuotationElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<blockquote>").Append(LineBreak);
            builder.Append("<div class=" + DoubleQuotes + "paragraph\">").Append(LineBreak);
            builder.Append("<p>");
            foreach (DocsElement child in element.Children)
            {
                builder.Append(child.Value);
            }

            builder.Append("</p>").Append(LineBreak);
            builder.Append("</div>").Append(LineBreak);
            builder.Append("</blockquote>").Append(LineBreak);
            //builder.Append(Visit(element.AttributeElement));
            return builder.ToString();
        }

        public string Visit(SideBarElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=").Append(DoubleQuotes).Append("side-bar").Append(DoubleQuotes).Append(">");

            foreach (DocsElement child in element.Children)
            {
                builder.Append("<pre>");
                builder.Append(child.Value);
                builder.Append("</pre>");
            }

            builder.Append("</div>");


            return builder.ToString();
        }

        public string Visit(TableElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table>").Append(LineBreak);
            {
                builder.Append("<thead>").Append(LineBreak);
                builder.Append("<tr>").Append(LineBreak);


                foreach (String columnHeading in element.ColumnHeading)
                {
                    builder.Append("<th>").Append(columnHeading).Append("</th>").Append(LineBreak);
                }

                builder.Append("</tr>").Append(LineBreak);
                builder.Append("</thead>").Append(LineBreak);
            }
            {
                builder.Append("<tbody>").Append(LineBreak);
                foreach (String[] row in element.Rows)
                {
                    builder.Append("<tr>").Append(LineBreak);
                    foreach (String column in row)
                    {
                        builder.Append("<td>").Append(column).Append("</td>").Append(LineBreak);
                    }

                    builder.Append("</tr>").Append(LineBreak);
                }

                builder.Append("</tbody>").Append(LineBreak);
            }

            builder.Append("</table>").Append(LineBreak);

            return builder.ToString();
        }

        public string Visit(TitleElement element)
        {
            return "<div class=" + DoubleQuotes + "title\">" + element.Title + "</div>";
        }

        public string Visit(UnOrderedListElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<ul>").Append(LineBreak);

            builder.Append("<li>").Append(LineBreak);
            builder.Append(element.Value);
            foreach (DocsElement docs in element.Children)
            {
                builder.Append(docs.Accept(this));
            }

            builder.Append("</li>").Append(LineBreak);

            builder.Append("</ul>").Append(LineBreak);

            return builder.ToString();
        }

        public string Visit(string s)
        {
            return "<p>" + s + "</p>";
        }

        public string Visit(PlainTextElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(element.Value);

            if (element.Next != null)
            {
                builder.Append(element.Next.Accept(this));
            }

            return builder.ToString();
        }

        public string Visit(object s)
        {
            return s.ToString();
        }
    }
}