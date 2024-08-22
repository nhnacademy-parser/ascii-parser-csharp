using System;
using System.Collections.Generic;
using System.Text;
using DocumentParser.Domain.Html;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Visitors.implementations
{
    public class HtmlConverter : IDocumentVisitor
    {
        private readonly List<FootNoteElement> _footNoteElements = new List<FootNoteElement>();

        public string Visit(DocsElement element)
        {
            return element.ToString();
        }

        public string Visit(AnchorElement element)
        {
            return HtmlTag.TagBlock("a", element.AltText, "href", element.Href);
        }

        public string Visit(BoldTextElement element)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(HtmlTag.TagBlock("strong", element.BoldText));

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
            return HtmlTag.TagBlock("a", element.AltText, "href", "#" + element.RefTarget);
        }


        public string Visit(ExampleBlockElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DocsElement child in element.Children)
            {
                builder.Append(HtmlTag.TagBlock("p", child.Accept(this).ToString()));
            }

            return HtmlTag.TagBlock("div", builder.ToString());
        }

        public string Visit(FootNoteElement element)
        {
            _footNoteElements.Add(element);
            StringBuilder builder = new StringBuilder();

            String footnoteId = "_footnoteref_" + _footNoteElements.Count;

            builder.Append("[")
                .Append(HtmlTag.TagBlock("a", _footNoteElements.Count.ToString(),
                    new KeyValuePair<string, string>("id", footnoteId),
                    new KeyValuePair<string, string>("class", "footnote"),
                    new KeyValuePair<string, string>("href", "#" + footnoteId)))
                .Append("]");

            return HtmlTag.TagBlock("sup", builder.ToString(), "class", "footnote");
        }

        public string Visit(HeadingElement element)
        {
            string tag = "h" + element.Level;
            string id = element.Heading.Replace(" ", "_").ToLower();

            return HtmlTag.TagBlock(tag, element.Heading, "id", id);
        }

        public string Visit(ImageElement element)
        {
            return HtmlTag.TagBlock("img", element.AltText,
                new KeyValuePair<string, string>("src", element.Href),
                new KeyValuePair<string, string>("alt", element.AltText)
            );
        }

        public string Visit(ItalicTextElement element)
        {
            StringBuilder builder = new StringBuilder();

            if (element.Next != null)
            {
                builder.Append(element.Next.Accept(this));
            }

            return HtmlTag.TagBlock("em", element.ItalicText) + builder;
        }

        public string Visit(ListingBlockElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DocsElement child in element.Children)
            {
                if (child is InlineElement { Value: DocsElement inlineElement })
                {
                    builder.Append(inlineElement.Accept(this)).Append('\n');
                }
                else
                {
                    builder.Append(child.Accept(this)).Append('\n');
                }
            }

            return
                HtmlTag.TagBlock("div",
                    HtmlTag.TagBlock("pre", builder.ToString())
                );
        }

        public string Visit(InlineElement element)
        {
            StringBuilder builder = new StringBuilder();

            if (element.Value is DocsElement child)
            {
                builder.Append(child.Accept(this));
            }

            return HtmlTag.TagBlock("p", builder.ToString());
        }

        public string Visit(OrderedListElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(element.Value);

            foreach (DocsElement docs in element.Children)
            {
                builder.Append(docs.Accept(this));
            }

            return
                HtmlTag.TagBlock("ol",
                    HtmlTag.TagBlock("li", builder.ToString()));
        }

        public string Visit(QuotationElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DocsElement child in element.Children)
            {
                builder.Append(child.Value);
            }

            return
                HtmlTag.TagBlock("blockquote",
                    HtmlTag.TagBlock("div",
                        HtmlTag.TagBlock("p", builder.ToString()), "class", "paragraph"));
        }

        public string Visit(SideBarElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DocsElement child in element.Children)
            {
                builder.Append(HtmlTag.TagBlock("p", child.Value.ToString()));
            }


            return HtmlTag.TagBlock("div", builder.ToString(), "class", "side-bar");
        }

        public string Visit(TableElement element)
        {
            StringBuilder table = new StringBuilder();


            StringBuilder columnHeading = new StringBuilder();
            foreach (String heading in element.ColumnHeading)
            {
                columnHeading.Append(HtmlTag.TagBlock("th", heading));
            }

            table.Append(
                HtmlTag.TagBlock("thead",
                    HtmlTag.TagBlock("tr", columnHeading.ToString()))
            );


            StringBuilder tr = new StringBuilder();

            foreach (String[] row in element.Rows)
            {
                StringBuilder td = new StringBuilder();
                foreach (String column in row)
                {
                    td.Append(HtmlTag.TagBlock("td", column));
                }

                tr.Append(HtmlTag.TagBlock("tr", td.ToString()));
            }

            table.Append(
                HtmlTag.TagBlock("tbody",
                    HtmlTag.TagBlock("tr", tr.ToString()))
            );

            return HtmlTag.TagBlock("table", table.ToString());
        }

        public string Visit(TitleElement element)
        {
            return HtmlTag.TagBlock("div", element.Title, "class", "title");
        }

        public string Visit(UnOrderedListElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(element.Value);

            foreach (DocsElement docs in element.Children)
            {
                builder.Append(docs.Accept(this));
            }

            return
                HtmlTag.TagBlock("ul",
                    HtmlTag.TagBlock("li", builder.ToString()));
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
    }
}