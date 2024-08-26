using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Parsers;
using DocumentParser.Parsers.Implementations;
using JetBrains.Annotations;

namespace DocumentParserTest.Parsers.Implementations;

[TestSubject(typeof(IDocumentParser))]
public class AsciiDoctorParserTest
{
    private readonly IDocumentParser _documentParser = new AsciiDoctorParser();

    [Fact]
    void Parse_One_Paragraph()
    {
        string input = "This is a basic AsciiDoc document.";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
    }

    [Fact]
    void Parse_Two_Paragraphs()
    {
        string input = "This is a basic AsciiDoc document.\n\nThis document contains two paragraphs.";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 2);
    }

    [Fact]
    void Parse_Three_Paragraphs()
    {
        string input =
            "= Document Title\n:reproducible:\n\nThis is a basic AsciiDoc document by {author}.\n\nThis document contains two paragraphs.\nIt also has a header that specifies the document title.\n";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 3);
    }
    
    [Fact]
    void Parse_Section()
    {
        string input = "== Section Title";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
        Assert.True(output[0] is SectionTitleElement);
    }

    [Fact]
    void Parse_AttributeEntry()
    {
        string input = ":name: value";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
        Assert.True(output[0] is AttributeEntryElement);
    }

    [Fact]
    void Parse_AttributeEntry_Extends()
    {
        string input = ":name: value \\\nmore value";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
        Assert.True(output[0] is AttributeEntryElement);
    }

    [Fact]
    void Parse_SidebarElement()
    {
        string input = "Text in your document.\n\n****\nThis is content in a sidebar block.\n\nimage::name.png[]\n\nThis is more content in the sidebar block.";
        
        List<IDocumentElement> output = _documentParser.Parse(input);
        
        Assert.NotNull(output);
        Assert.True(output.Count == 2);
        Assert.True(output[1] is SideBarBlockElement);
    }
    
    private static Stream GetStream(string filename)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream? stream = assembly.GetManifestResourceStream("DocumentParserTest.Resources.Asciidocs.template.adoc");

        Debug.Assert(stream != null, nameof(stream) + " != null");
        return stream;
    }
} 