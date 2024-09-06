using DocumentParser.Domains.Htmls;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace DocumentParserTest.Domains.Html;

[TestSubject(typeof(HtmlTag))]
public class HtmlTagTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public HtmlTagTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void METHOD()
    {
        string h1 = HtmlTag.TagBlock("h1", "title",
            new TagAttribute("class", "title", "href")
        );

        _testOutputHelper.WriteLine(h1);
    }
}