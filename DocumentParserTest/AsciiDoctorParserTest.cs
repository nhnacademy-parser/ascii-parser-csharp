using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using DocumentParser.Domain;
using DocumentParser.Elements.Implementations;
using DocumentParser.Visitors.implementations;
using DocumentParser.Parsers.Implementations;

namespace ParserTest;

public class AsciiDoctorParserTest
{
    //[Fact]
    public void ParseUsingIfStatement()
    {
        
        FileStream sourceFileStream = File.OpenRead("../../../resources/asciidocs/template.adoc");
        StreamReader inputStreamReader = new StreamReader(sourceFileStream);

        FileStream outputFileStream = File.Create("../../../resources/asciidocs/test.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);



        List<DocsElement> docsElements = new List<DocsElement>();


        while (!inputStreamReader.EndOfStream)
        {
            string s = inputStreamReader.ReadLine();

            if (s == null)
            {
                break;
            }

            DocsElement docsElement = new DocsElement(s);

            if (s.StartsWith("="))
            {    //heading or Example Block

                string exampleBlockstring = "====$";
                Match match = Regex.Match(s, exampleBlockstring);

                if (match.Success)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    while (!inputStreamReader.EndOfStream)
                    {
                        s = inputStreamReader.ReadLine();
                        match = Regex.Match(s, exampleBlockstring);
                        if (match.Success)
                        {
                            break;
                        }
                        stringBuilder.Append(s).Append('\n');
                    }
                    docsElement = new ExampleBlockElement(stringBuilder.ToString());
                }
                else
                {
                    Regex regex = new Regex("=");

                    docsElement = new HeadingElement(regex.Replace(s, (match) => ""), regex.Matches(s).Count);
                }
            }
            else if (s.StartsWith("."))
            {// title or orderList

                Regex titleRegex = new Regex("^\\.[^ .]");
                Match match = titleRegex.Match(s);

                if (match.Success)
                { // title
                    docsElement = new TitleElement(s[1..]);
                }
                else
                {
                    Regex orderListRegex = new Regex("\\.");

                    docsElement = new OrderedListElement(new List<DocsElement>(), orderListRegex.Matches(s).Count);
                }
            }
            else if (s.StartsWith("*"))
            { // orderList or sideBar

                Regex sideBarRegex = new Regex("\\*\\*\\*\\*$");
                Match match = sideBarRegex.Match(s);

                if (match.Success)
                { // sideBar
                    StringBuilder stringBuilder = new StringBuilder();
                    while (!inputStreamReader.EndOfStream)
                    {
                        s = inputStreamReader.ReadLine();
                        if (sideBarRegex.Match(s).Success)
                        {
                            break;
                        }
                        stringBuilder.Append(s).Append("\n");
                    }
                    docsElement = new SideBarElement(stringBuilder.ToString());
                }
                else
                { // unordered List
                    Regex regex = new Regex("\\*");
                    int level = regex.Matches(s).Count;
                    docsElement = new UnOrderedListElement(new List<DocsElement>(), level);
                }

            }
            else if (s.StartsWith("-"))
            {// unorderedList(st. Markdown) , listing Block

                Regex listingBlockRegex = new Regex("----$");
                Match match = listingBlockRegex.Match(s);

                if (match.Success)
                { // listing Block
                    StringBuilder stringBuilder = new StringBuilder();
                    while (!inputStreamReader.EndOfStream)
                    {
                        s = inputStreamReader.ReadLine();
                        if (listingBlockRegex.Match(s).Success)
                        {
                            break;
                        }
                        stringBuilder.Append(s).Append("\n");
                    }
                    docsElement = new ListingBlockElement();
                }
                else
                {  // unorderedList(st. Markdown)
                    Regex regex = new Regex("-");
                    int level = regex.Matches(s).Count;
                    docsElement = new UnOrderedListElement(new List<DocsElement>(), level);
                }

            }
            else if (s.StartsWith("image::"))
            { // image

                Regex imageRefRegex = new Regex("^image::");
                Match match = imageRefRegex.Match(s);

                if (match.Success)
                {
                    String temp = imageRefRegex.Replace(s,"");

                    Regex altTextRegex = new Regex("\\[.+]");
                    match = altTextRegex.Match(temp);

                    if (match.Success)
                    {
                        String alt = match.Value;

                        temp = altTextRegex.Replace(temp, "");
                        docsElement = new ImageElement(temp, alt.Substring(1, alt.Length -2));
                    }

                }
            }
            else if (s.StartsWith("|==="))
            {

                Regex tableRegex = new Regex("\\|===$");
                Match match = tableRegex.Match(s);

                if (match.Success)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    while (!inputStreamReader.EndOfStream)
                    {
                        s = inputStreamReader.ReadLine();
                        if (tableRegex.Match(s).Success)
                        {
                            break;
                        }
                        stringBuilder.Append(s).Append("\n");
                    }
                    docsElement = new TableElement() ;
                }

            }
            else if (s.StartsWith("["))
            { // Quotation Attribute
                Regex attributeRegex = new Regex("\\[.+]$");
                Match match = attributeRegex.Match(s);

                if (match.Success)
                {
                    String attributes = match.Value;
                    String[] temp = new String[3];
                    String[] copy = attributes.Substring(1, attributes.Length - 2).Split(",");

                    Array.Copy(copy, 0, temp, 0, Math.Min(copy.Length, 3));

                    Regex regex = new Regex("^#");
                    Match match1 = regex.Match(temp[0]);

                    if (match1.Success)
                    {
                        // Attribute 에 대한 재정의 필
                        continue;
                    }


                    docsElement = new AttributeElement(temp[0], temp[1], temp[2]);
                }

            }
            else if (s.StartsWith("_"))
            { // Quotation

                Regex quoteRegex = new Regex("____");
                Match match = quoteRegex.Match(s);

                if (match.Success)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    while (!inputStreamReader.EndOfStream)
                    {
                        s = inputStreamReader.ReadLine();
                        if (quoteRegex.Match(s).Success)
                        {
                            break;
                        }
                        stringBuilder.Append(s).Append("\n");
                    }
                    QuotationElement element = new QuotationElement(stringBuilder.ToString());

                    int i = docsElements.Count - 1;

                    DocsElement lastElement = docsElements.ElementAt(i);
                    docsElements.RemoveAt(i);

                    if (lastElement is AttributeElement attributeElement) {
                        element.AttributeElement = attributeElement;
                    }
                    docsElement = element;
                }

            }
            else if (s.StartsWith("//"))
            { // Comment
                Regex commentRegex = new Regex("//");
                Match match = commentRegex.Match(s);

                if (match.Success)
                {
                    docsElement = new CommentElement(commentRegex.Replace(s, ""));
                }

            }
            else if (s.StartsWith(":"))
            {

            }
            else if (s.Contains("<<"))
            { //
                Regex crossRefRegex = new Regex("<<+.+>>");
                Match match = crossRefRegex.Match(s);

                if (match.Success)
                {
                    string temp = match.Value;
                    docsElements.Add(new DocsElement(crossRefRegex.Replace(s, "")));

                    Regex angleBracketRegex = new Regex("[<>]");
                    Match match1 = angleBracketRegex.Match(temp);
                    temp = angleBracketRegex.Replace(temp, "");
                    string[] strings = temp.Split(",");

                    docsElement = new CrossReferenceElement(strings[0], strings[1]);
                }
            }
            else if (s.Contains("://"))
            { // protocol ref
                Regex protocolIncludeAltTextRegex = new Regex("(\\S+://)(.+)(\\[(.*)])");
                Regex protocolExcludeAltTextRegex = new Regex("(\\S+://[^ ]+)");

                Match match = protocolIncludeAltTextRegex.Match(s);

                if (match.Success)
                {
                    string href = match.Groups[1].Value + match.Groups[2].Value;
                    string altText = match.Groups[4].Value;

                    docsElements.Add(new DocsElement(s[..match.Index]));
                    docsElements.Add(new AnchorElement(href, altText));
                    docsElements.Add(new DocsElement(s.Substring(match.Index + match.Length)));
                    continue;

                }
                else if ((match = protocolExcludeAltTextRegex.Match(s)).Success)
                {
                    //match = new Regex("(\\S+://[^ ])+ $").Match(s)
                    string href = match.Groups[1].Value;
                    docsElements.Add(new DocsElement(s[..match.Index]));
                    docsElements.Add(new AnchorElement(href));
                    docsElements.Add(new DocsElement(s.Substring(match.Index + match.Length)));
                    continue;
                }

            }
            else if (s.Contains("*"))
            { // 좌우 스플릿 후 recursion 형태로 해야할 필요성이 보임.
                Regex boldRegex = new Regex("\\*(.*)\\*");

                Match match = boldRegex.Match(s);

                if (match.Success)
                {
                    docsElements.Add(new DocsElement(s[..match.Index]));
                    docsElements.Add(new DocsElement(match.Groups[1].Value));
                    docsElements.Add(new DocsElement(s.Substring(match.Index + match.Length)));
                    continue;
                }
            }
            else if (s.Contains("_"))
            {
                Regex italicRegex = new Regex("_(.*)_");
                Match match = italicRegex.Match(s);

                if (match.Success)
                {
                    docsElements.Add(new DocsElement(s[..match.Index]));
                    docsElements.Add(new DocsElement(match.Groups[1].Value));
                    docsElements.Add(new DocsElement(s.Substring(match.Index + match.Length)));
                    continue;
                }
            }
            else if (s.Contains("footnote:"))
            {
                Regex footnoteRegex = new Regex("footnote:\\[(.*)]");
                Match match = footnoteRegex.Match(s);

                if (match.Success)
                {
                    docsElements.Add(new DocsElement(s[..match.Index]));
                    docsElements.Add(new DocsElement(match.Groups[1].Value));
                    docsElements.Add(new DocsElement(s.Substring(match.Index + match.Length)));
                    continue;
                }
            }

            docsElements.Add(docsElement);
        }

        StringBuilder builder = new StringBuilder();

        HtmlConverter htmlConverter = new HtmlConverter();

        foreach (DocsElement docsElement in docsElements)
        {
            string str = docsElement.Accept(htmlConverter).ToString();
            builder.Append(str).Append('\n');
        }

        outputStreamWriter.Write(builder.ToString());
        outputStreamWriter.Flush();
    }


    //[Fact]
    public void ParseUsingAsciiDocsParser()
    {
        AsciiDocsParser asciiDocsParser = new AsciiDocsParser();

        Document document = asciiDocsParser.LoadFile("../../../resources/asciidocs/template.adoc");

        FileStream outputFileStream = File.Create("../../../resources/asciidocs/parseTree.txt");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);

        outputStreamWriter.Write(document.ToString());
        outputStreamWriter.Flush();
    }

    [Fact]
    public void ConvertToHtml()
    {
        AsciiDocsParser asciiDocsParser = new AsciiDocsParser();

        Document document = asciiDocsParser.LoadFile("../../../resources/asciidocs/template.adoc");

        FileStream outputFileStream = File.Create("../../../resources/asciidocs/test.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);

        HtmlConverter htmlConverter = new HtmlConverter();

        string htmlDocument = document.convert(htmlConverter);

        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }
}
