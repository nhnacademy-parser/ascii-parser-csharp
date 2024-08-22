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
    public void ConvertToHtml()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream? stream = assembly.GetManifestResourceStream("DocumentParserTest.Resources.Asciidocs.template.adoc");

        string htmlDocument = new AsciiDocsParser().LoadFile(stream).Convert(new HtmlConverter());

        string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/downloads";
        FileStream outputFileStream = File.Create(userHomePath + "/test.html");
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

        string htmlDocument = new AsciiDocsParser().LoadFile(response.Content.ReadAsStream())
            .Convert(new HtmlConverter());

        string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/downloads";
        FileStream outputFileStream = File.Create(userHomePath + "/test.html");
        StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);
        outputStreamWriter.Write(htmlDocument);
        outputStreamWriter.Flush();
    }
}