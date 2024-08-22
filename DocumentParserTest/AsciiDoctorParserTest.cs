using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using DocumentParser.Domain;
using DocumentParser.Elements.Implementations;
using DocumentParser.Parsers.Implementations;
using DocumentParser.Visitors.implementations;

namespace ParserTest;

public class AsciiDoctorParserTest
{
    [Fact]
    public void ConvertToHtml()
    {
        AsciiDocsParser asciiDocsParser = new AsciiDocsParser();
        
        Document document = asciiDocsParser.LoadFile("../../../resources/asciidocs/template.adoc");

        FileStream outputFileStream = File.Create("../../../resources/asciidocs/test.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);

        HtmlConverter htmlConverter = new HtmlConverter();

        string htmlDocument = document.Convert(htmlConverter);

        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }
    [Fact]
    public void ConvertWebAsciiToHtml()
    {
        AsciiDocsParser asciiDocsParser = new AsciiDocsParser();
        
        WebClient webClient = new WebClient();
        
        webClient.DownloadFile("https://raw.githubusercontent.com/gikpreet/class-programming_with_java/master/Module%2004%20Statement%EC%99%80%20Exception/homework/maze.adoc", "../../../resources/asciidocs/download.adoc");
        
        Document document = asciiDocsParser.LoadFile("../../../resources/asciidocs/download.adoc");

        FileStream outputFileStream = File.Create("../../../resources/asciidocs/test.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);

        HtmlConverter htmlConverter = new HtmlConverter();

        string htmlDocument = document.Convert(htmlConverter);

        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }
    
    
}
