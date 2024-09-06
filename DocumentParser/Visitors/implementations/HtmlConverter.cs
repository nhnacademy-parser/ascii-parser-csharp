using System;
using System.Collections.Generic;
using System.Text;
using DocumentParser.Domain;
using DocumentParser.Domains.Htmls;
using DocumentParser.Domains.Nodes;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Blocks.Lists;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.Visitors.implementations
{
    public class HtmlConverter : IDocumentVisitor
    {
        public string Visit(IDocumentElement element)
        {
            return HtmlTag.TagBlock("p", element.ToString());
        }

        public string Visit(LineElement element)
        {
            return HtmlTag.TagBlock("span", element.Value);
        }

        public string Visit(InlineElement element)
        {
            StringBuilder builder = new StringBuilder();

            LineElement line = element.Children[0] as LineElement;

            foreach (LineElement n in line)
            {
                builder.Append((n as IDocumentElement).Accept(this));
            }

            return HtmlTag.TagBlock("p", builder.ToString());
        }

        public string Visit(BoldTextElement element)
        {
            return HtmlTag.TagBlock("strong", element.BoldText);
        }

        public string Visit(ItalicTextElement element)
        {
            return HtmlTag.TagBlock("em", element.ItalicText);
        }

        public string Visit(ParagraphElement element)
        {
            return element.Paragraph;
        }

        public string Visit(SectionTitleElement element)
        {
            string id = "_" + element.Title.ToLower().Replace(" ", "_");
            string tagName = "h" + element.Level;

            return HtmlTag.TagBlock(tagName, element.Title,
                list =>
                {
                    list.Add(new TagAttribute("id", id));
                    list.Add(new TagAttribute("class", "title"));
                });
        }

        public string Visit(QuotationBlockElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IDocumentElement e in element.Children)
            {
                if (e is InlineElement inline)
                {
                    builder.Append(inline.Value.Accept(this)).Append(" ");
                }
                else
                {
                    builder.Append(e.Accept(this)).Append(" ");
                }
            }

            return HtmlTag.TagBlock("blockqute",
                HtmlTag.TagBlock("span", builder.ToString()), "class", "blockquote");
        }

        public string Visit(ListElement element)
        {
            return HtmlTag.TagBlock("li", element.Value.Accept(this));
        }

        public string Visit(ListContainerElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IDocumentElement e in element.Children)
            {
                builder.Append(e.Accept(this));
            }

            return HtmlTag.TagBlock("ul", builder.ToString());
        }

        public string Visit(TitleElement element)
        {
            string content = HtmlTag.TagBlock("div", element.Value.Accept(this));
            string title =
                HtmlTag.TagBlock("em",
                    HtmlTag.TagBlock("strong", element.Title, "class", "title"));

            return HtmlTag.TagBlock("div", title + content, "class", "title");
        }

        public string Visit(BlockElement element)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IDocumentElement e in element.Children)
            {
                builder.Append(e.Accept(this));
            }

            return HtmlTag.TagBlock("div", builder.ToString());
        }

        public string Visit(ImageReferenceElement element)
        {
            return HtmlTag.TagBlock("img", string.Empty,
                list =>
                {
                    list.Add(new TagAttribute("href", element.ImageReference));
                    list.Add(new TagAttribute("alt", element.AltText));
                });
        }

        public string Visit(AnchorElement element)
        {
            return HtmlTag.TagBlock("a", element.AltText, "href", element.Reference);
        }

        public string Visit(CrossReferenceElement element)
        {
            return HtmlTag.TagBlock("a", element.AltText, "href", "#" + element.CrossReference);
        }

        public string Visit(CommentElement element)
        {
            return "<!--" + element.Comment + "-->";
        }

        public string Convert(Document document)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IDocumentElement element in document.Body)
            {
                builder.Append(element.Accept(this));
            }

            return builder.ToString();
        }
    }
}