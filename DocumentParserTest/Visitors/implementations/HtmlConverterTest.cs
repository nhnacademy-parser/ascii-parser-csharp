using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Web;
using DocumentParser.Domain;
using DocumentParser.Parsers.Implementations;
using DocumentParser.Visitors.implementations;
using JetBrains.Annotations;

namespace DocumentParserTest.Visitors.implementations;

[TestSubject(typeof(HtmlConverter))]
public class HtmlConverterTest
{
    [Fact]
    public void ParseStringConvertToHtml()
    {
        string input =
            ".Unordered list title\n" +
            "* list item 1\n" + // 1
            "** nested list item\n" +
            "*** nested nested list \n" +
            "item 1\n" +
            "\n" +
            "*** nested nested list " +
            "\n" +
            "\n" +
            "item 2" + // 2
            "\n" +
            "* list item 2"; // 3


        Document document = new Document();
        document.Body = new AsciiDoctorParser().Parse(input);
        string htmlDocument = new HtmlConverter().Convert(document);

        string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/downloads";
        FileStream outputFileStream = File.Create(userHomePath + "/document-parser-test-on-project-resource.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);
        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }


    [Fact]
    public void ConvertToHtml()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream? stream = assembly.GetManifestResourceStream("DocumentParserTest.Resources.Asciidocs.template.adoc");

        Document document = new AsciiDoctorParser().LoadFile(stream);
        string htmlDocument = new HtmlConverter().Convert(document);

        string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/downloads";
        FileStream outputFileStream = File.Create(userHomePath + "/document-parser-test-on-project-resource.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);
        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }

    // [Fact]
    public void ConvertWebAsciiToHtml()
    {
        string uri =
            "https://raw.githubusercontent.com/gikpreet/class-programming_with_java/master/Module%2004%20Statement%EC%99%80%20Exception/homework/maze.adoc";
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, uri));

        Document document = new AsciiDoctorParser().LoadFile(response.Content.ReadAsStream());
        string htmlDocument = new HtmlConverter().Convert(document);

        string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/downloads";
        FileStream outputFileStream = File.Create(userHomePath + "/document-parser-test-on-web.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);
        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }
}