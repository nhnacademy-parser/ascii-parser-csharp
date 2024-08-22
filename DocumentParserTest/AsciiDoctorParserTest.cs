using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Xunit.Abstractions;

namespace DocumentParserTest;

public class AsciiDoctorParserTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AsciiDoctorParserTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void res()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream? fileStream = assembly.GetManifestResourceStream("DocumentParserTest.Resources.Asciidocs.template.adoc");

        Debug.Assert(fileStream != null, nameof(fileStream) + " != null");

        StreamReader streamReader = new StreamReader(fileStream);

        _testOutputHelper.WriteLine(streamReader.ReadToEnd());
    }

    [Fact]
    void Download()
    {
        string uri =
            "https://raw.githubusercontent.com/gikpreet/class-programming_with_java/master/Module%2004%20Statement%EC%99%80%20Exception/homework/maze.adoc";

        HttpClient httpClient = new HttpClient();

        HttpResponseMessage response = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, uri));
        StreamReader streamReader = new StreamReader(response.Content.ReadAsStream());

        _testOutputHelper.WriteLine(streamReader.ReadToEnd());
    }
}