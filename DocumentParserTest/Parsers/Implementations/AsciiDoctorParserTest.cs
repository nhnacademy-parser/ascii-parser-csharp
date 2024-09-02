using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Addition;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Blocks.Lists;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;
using DocumentParser.Parsers;
using DocumentParser.Parsers.Implementations;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace DocumentParserTest.Parsers.Implementations;

[TestSubject(typeof(IDocumentParser))]
public class AsciiDoctorParserTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IDocumentParser _documentParser = new AsciiDoctorParser();

    public AsciiDoctorParserTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void Parse_One_Paragraph()
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
        Assert.True(output.Count == 4);
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
        Assert.True(output.Count == 0);
    }

    [Fact]
    void Parse_AttributeEntry_Extends()
    {
        string input = ":name: value \\\nmore value";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 0);
    }

    [Fact]
    void Parse_SidebarElement()
    {
        string input =
            "Text in your document.\n\n****\nThis is content in a sidebar block.\n\nimage::name.png[]\n\nThis is more content in the sidebar block.\n****";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 2);
        Assert.True(output[1] is SideBarBlockElement);
    }

    [Fact]
    void Parse_SidebarElement_second()
    {
        string input =
            "Text in your document.\n\n****\nThis is content in a sidebar block.\n\nimage::name.png[]\n\nThis is more content in the sidebar block.\n****\nadditional paragraphs.";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 3);
        Assert.True(output[1] is SideBarBlockElement);
    }

    [Fact]
    void Parse_SidebarElement_unclosed()
    {
        string input =
            "Text in your document.\n\n****\nThis is content in a sidebar block.\n\nimage::name.png[]\n\nThis is more content in the sidebar block.";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 2);
        Assert.True(output[1] is SideBarBlockElement);
    }

    [Fact]
    void Parse_Nesting_Block()
    {
        string input =
            "====\nHere's a sample AsciiDoc document:\n\n----\n= Document Title\nAuthor Name\n\nContent goes here.\n----\n\nThe document header is useful, but not required.\n====";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
        Assert.True(output[0] is ExampleBlockElement);
        Assert.True(((output[0] as ExampleBlockElement)!).Children.Count == 3);
    }

    [Fact]
    void Parse_Nesting_SameStructuralBlock()
    {
        string input =
            "====\n" +
            "Here are your options:\n" +
            "\n" +
            ".Red Pill\n" +
            "[%collapsible]\n" +
            "======\n" +
            "Escape into the real world.\n" +
            "======\n\n" +
            ".Blue Pill\n" +
            "[%collapsible]\n" +
            "======\n" +
            "Live within the simulated reality without want or fear.\n" +
            "======\n" +
            "====\n";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
        Assert.True(output[0] is ExampleBlockElement);
        Assert.True(((output[0] as ExampleBlockElement)!).Children.Count == 3);
    }

    [Fact]
    void Parse_Nesting_SpecialBlock()
    {
        string input =
            "[%collapsible]\n" +
            "======\n" +
            "Escape into the real world.\n" +
            "======\n\n" +
            ".Blue Pill\n" +
            "[%collapsible]\n" +
            "======\n" +
            "Live within the simulated reality without want or fear.\n" +
            "======";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 2);
        Assert.True(output[0] is SpecialBlockElement);
        Assert.True(((output[0] as BlockElement)!).Children.Count == 1);
    }

    [Fact]
    void Parse_ListElement()
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

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 3);
        Assert.True(output[0] is TitleElement);
        Assert.True((output[0] as TitleElement).Children.Count == 1);
        Assert.True(output[1] is ParagraphElement);
        Assert.True(output[2] is ListContainerElement);
    }
    [Fact]
    void Parse_OrderedListElement()
    {
        string input = 
            ". ordered list item 1\n" +
            ".. nested ordered list item 1 - 1\n" + 
            "... nested nested ordered list item 1 - 1 - 1\n" + 
            "... nested nested ordered list item 1 - 1 - 2\n" +
            ".. nested ordered list item 1 - 2\n" +
            "... nested nested ordered list item 1 - 2 - 1\n" + 
            ". ordered list item 2\n" +
            ". ordered list item 3\n";

        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 1);
    }

    [Fact]
    void Table_Element()
    {
        string input = "===== Fourth level heading\n\n.Table title\n|===\n|Column heading 1 |Column heading 2\n\n|Column 1, row 1\n|Column 2, row 1\n\n|Column 1, row 2\n|Column 2, row 2\n|===\n";
        
        List<IDocumentElement> output = _documentParser.Parse(input);

        Assert.NotNull(output);
        Assert.True(output.Count == 2);
        Assert.True(output[1] is TitleElement);
        Assert.True(output[0] is SectionTitleElement);
    }

    // [Fact]
    void Parse_Adoc()
    {
        string filename = "DocumentParserTest.Resources.Asciidocs.template.adoc";

        Stream stream = GetStream(filename);

        IDocumentParser parser = new AsciiDoctorParser();

        Document document = parser.LoadFile(stream);

        Assert.NotNull(document);
    }

    private static Stream GetStream(string filename)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream? stream = assembly.GetManifestResourceStream(filename);

        Debug.Assert(stream != null, nameof(stream) + " != null");
        return stream;
    }
}