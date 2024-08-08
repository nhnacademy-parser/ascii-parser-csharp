using System;
using System.IO;
using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Parsers;
using DocumentParser.Elements.Implementations;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace DocumentParser.Parsers.Implementations
{
	public class AsciiDocsParser:IDocumentParser
	{
		public AsciiDocsParser()
		{
		}

        public Document LoadFile(string filePath)
        {
            return LoadFile(File.OpenRead(filePath));
        }

        public Document LoadFile(FileStream file)
        {
            Document document = new Document();

            DocsElement headElement = new DocsElement();
            document.AddRootElement(headElement);

            StreamReader inputStreamReader = new StreamReader(file);

            while(!inputStreamReader.EndOfStream)
            { 
                string str = inputStreamReader.ReadLine();

                DocsElement element = Parse(str);

                bool flag = false;
                for (; headElement.EndOfContaier(element); headElement = headElement.Parent)
                {
                    flag = true;
                }

                headElement.AddChild(element);

                if (flag)
                {
                    continue;
                }


                if (element.IsContainer)
                { 
                    element.Parent = headElement;
                    headElement = element;
                }
            }

            return document;
        }

        public DocsElement Parse(string document)
        {

            if (document.StartsWith("="))
            {    //heading or Example Block

                string exampleBlockstring = "====$";
                Match match = Regex.Match(document, exampleBlockstring);

                if (match.Success)
                {
                    return new ExampleBlockElement(document);
                }
                else
                {
                    Regex regex = new Regex("=");
                    return new HeadingElement(Parse(regex.Replace(document, (match) => "")), regex.Matches(document).Count);
                }
            }
            else if (document.StartsWith("."))
            {// title or orderList

                Regex titleRegex = new Regex("^\\.[^ .]");
                Match match = titleRegex.Match(document);

                if (match.Success)
                { // title
                    return new TitleElement(document[1..]);
                }
                else
                {
                    Regex orderListRegex = new Regex("\\.");

                    return new OrderedListElement(Parse(orderListRegex.Replace(document, (match) => "")), orderListRegex.Matches(document).Count);
                }
            }
            else if (document.StartsWith("*"))
            { // orderList or sideBar

                Regex sideBarRegex = new Regex("\\*\\*\\*\\*$");
                Match match = sideBarRegex.Match(document);

                if (match.Success)
                { // sideBar
                    return new SideBarElement(document);
                }
                else
                { // unordered List
                    Regex regex = new Regex("\\*");
                    return new UnOrderedListElement(Parse(regex.Replace(document, (match) => "")), regex.Matches(document).Count);
                }

            }
            else if (document.StartsWith("-"))
            {// unorderedList(st. Markdown) , listing Block

                Regex listingBlockRegex = new Regex("----$");
                Match match = listingBlockRegex.Match(document);

                if (match.Success)
                { // listing Block
                    return new ListingBlockElement();
                }
                else
                {  // unorderedList(st. Markdown)
                    Regex regex = new Regex("-");
                    return new UnOrderedListElement(Parse(regex.Replace(document, (match) => "")), regex.Matches(document).Count);
                }

            }
            else if (document.StartsWith("image::"))
            { // image

                Regex imageRefRegex = new Regex("^image::");
                Match match = imageRefRegex.Match(document);

                if (match.Success)
                {
                    String temp = imageRefRegex.Replace(document, "");

                    Regex altTextRegex = new Regex("\\[.+]");
                    match = altTextRegex.Match(temp);

                    if (match.Success)
                    {
                        String alt = match.Value;

                        temp = altTextRegex.Replace(temp, "");
                        return new ImageElement(temp, alt.Substring(1, alt.Length - 2));
                    }
                }
            }
            else if (document.StartsWith("|==="))
            {

                Regex tableRegex = new Regex("\\|===$");
                Match match = tableRegex.Match(document);

                if (match.Success)
                {
                    return new TableElement() ;
                }

            }
            else if (document.StartsWith("["))
            { // Quotation Attribute
                Regex attributeRegex = new Regex("\\[.+]$");
                Match match = attributeRegex.Match(document);

                if (match.Success)
                {
                    string attributes = match.Value;
                    string[] temp = new String[3];
                    string[] copy = attributes.Substring(1, attributes.Length - 2).Split(",");

                    Array.Copy(copy, 0, temp, 0, Math.Min(copy.Length, 3));

                    Regex regex = new Regex("^#");
                    Match match1 = regex.Match(temp[0]);

                    if (match1.Success)
                    {
                        // Attribute 에 대한 재정의 필요
                        return new IdAttributeElement(regex.Replace(temp[0],"").Replace("- ","_"));
                    }


                    return new AttributeElement(temp[0], temp[1], temp[2]);
                }

            }
            else if (document.StartsWith("_"))
            { // Quotation

                Regex quoteRegex = new Regex("____");
                Match match = quoteRegex.Match(document);

                if (match.Success)
                {
                    return new QuotationElement(document);
                }

            }
            else if (document.StartsWith("//"))
            { // Comment
                Regex commentRegex = new Regex("//");
                Match match = commentRegex.Match(document);

                if (match.Success)
                {
                    return new CommentElement(commentRegex.Replace(document, ""));
                }

            }
            else if (document.StartsWith(":"))
            {
                return new DocsElement(document);
            }
            else if (document.Contains("<<"))
            { //
                Regex crossRefRegex = new Regex("<<+.+>>");
                Match match = crossRefRegex.Match(document);

                if (match.Success)
                {

                    string temp = new Regex("[<>]").Replace(match.Value, "");
                    string[] strings = temp.Split(",");


                    DocsElement element = Parse(document[..match.Index]);
                    element.Append(new CrossReferenceElement(strings[0], strings[1])).Append(Parse(document[(match.Index + match.Length)..]));

                    return element;
                }
            }
            else if (document.Contains("://"))
            { // protocol ref
                Regex protocolIncludeAltTextRegex = new Regex("(\\S+://)(.+)(\\[(.*)])");
                Regex protocolExcludeAltTextRegex = new Regex("(\\S+://[^ ]+)");

                Match match = protocolIncludeAltTextRegex.Match(document);

                if (match.Success)
                {
                    string href = match.Groups[1].Value + match.Groups[2].Value;
                    string altText = match.Groups[4].Value;

                    DocsElement element = Parse(document[..match.Index]);
                    element.Append(new AnchorElement(href, altText)).Append(Parse(document[(match.Index + match.Length)..]));

                    return element;

                }
                else if ((match = protocolExcludeAltTextRegex.Match(document)).Success)
                {
                    //match = new Regex("(\\S+://[^ ])+ $").Match(s)
                    string href = match.Groups[1].Value;

                    DocsElement element = Parse(document[..match.Index]);
                    element.Append(new AnchorElement(href)).Append(Parse(document[(match.Index + match.Length)..]));

                    return element;

                }

            }
            else if (document.Contains("*"))
            { // 좌우 스플릿 후 recursion 형태로 해야할 필요성이 보임.
                Regex boldRegex = new Regex("\\*(.*)\\*");

                Match match = boldRegex.Match(document);

                if (match.Success)
                {
                    DocsElement element = Parse(document[..match.Index]);
                    element.Append(new BoldTextElement(Parse(match.Groups[1].Value))).Append(Parse(document[(match.Index + match.Length)..]));

                    return element;
                }

            }
            else if (document.Contains("_"))
            {
                Regex italicRegex = new Regex("_(.*)_");
                Match match = italicRegex.Match(document);

                if (match.Success)
                {
                    DocsElement element = Parse(document[..match.Index]);
                    element.Append(new ItalicTextElement(Parse(match.Groups[1].Value))).Append(Parse(document[(match.Index + match.Length)..]));

                    return element;
                }
            }
            else if (document.Contains("footnote:"))
            {
                Regex footnoteRegex = new Regex("footnote:\\[(.*)]");
                Match match = footnoteRegex.Match(document);

                if (match.Success)
                {
                    DocsElement element = Parse(document[..match.Index]);
                    element.Append(new FootNoteElement(Parse(match.Groups[1].Value))).Append(Parse(document[(match.Index + match.Length)..]));
                    return element;
                }
            }
            return new LineElement(document);
        }
    }
}

