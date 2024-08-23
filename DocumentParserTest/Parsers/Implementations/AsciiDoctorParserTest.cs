using System.Diagnostics;
using System.Reflection;
using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Parsers;
using DocumentParser.Parsers.Implementations;
using JetBrains.Annotations;

namespace DocumentParserTest.Parsers.Implementations;

[TestSubject(typeof(IDocumentParser))]
public class AsciiDoctorParserTest
{
    private readonly IDocumentParser _documentParser = new AsciiDocsParser();

    private static Stream GetStream(string filename)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream? stream = assembly.GetManifestResourceStream("DocumentParserTest.Resources.Asciidocs.template.adoc");

        Debug.Assert(stream != null, nameof(stream) + " != null");
        return stream;
    }

    [Fact]
    public void ParseHeader()
    {
        Stream reader = GetStream("DocumentParserTest.Resources.Asciidocs.header.adoc");

        Document document = _documentParser.LoadFile(reader);

        Assert.NotNull(document);
        Assert.NotNull(document.Header);
    }

    [Fact]
    public void ParseBody()
    {
        Stream reader = GetStream("DocumentParserTest.Resources.Asciidocs.header.adoc");

        Document document = _documentParser.LoadFile(reader);

        Assert.NotNull(document);
        Assert.NotNull(document.Body);
    }

    [Fact]
    public void ParseFooter()
    {
        Stream reader = GetStream("DocumentParserTest.Resources.Asciidocs.footer.adoc");

        Document document = _documentParser.LoadFile(reader);

        Assert.NotNull(document);
        Assert.NotNull(document.Footer);
    }

    [Fact]
    public void ParseHeading()
    {
        IDocumentElement element = _documentParser.Parse("= AsciiDoc Article Title");

        Assert.NotNull(element);
        Assert.True(element is HeadingElement);
    }

    [Fact]
    public void ParseTitle()
    {
        IDocumentElement element = _documentParser.Parse(".title");

        Assert.NotNull(element);
        Assert.True(element is TitleElement);
    }

    [Fact]
    public void ParseUnOrderedList()
    {
        IDocumentElement element = _documentParser.Parse(". ordered list item");

        Assert.NotNull(element);
        Assert.True(element is ListElement);
    }

    [Fact]
    public void ParseOrderedList()
    {
        IDocumentElement element = _documentParser.Parse("* unordered list item");

        Assert.NotNull(element);
        Assert.True(element is ListElement);
    }

    [Fact]
    public void ParseComment()
    {
        IDocumentElement element = _documentParser.Parse("// I am a comment and won't be rendered.");

        Assert.NotNull(element);
        Assert.True(element is CommentElement);
    }

    [Fact]
    public void ParseImageReference()
    {
        IDocumentElement element = _documentParser.Parse("image::image-file-name.png[I am the image alt text.]");

        Assert.NotNull(element);
        Assert.True(element is ImageReferenceElement);
    }

    [Fact]
    public void ParsePlainText()
    {
        IDocumentElement element =
            _documentParser.Parse("This is a paragraph with a *bold* word and an _italicized_ word.");

        Assert.NotNull(element);
        Assert.True(element is InlineElement);
    }


    [Fact]
    public void ParseFootnote()
    {
        IDocumentElement element = _documentParser.Parse(
            "This is another paragraph.footnote:[I am footnote text and will be displayed at the bottom of the article.]");

        Assert.NotNull(element);
        Assert.True(element is InlineElement);
    }


    [Fact]
    public void ParseExampleBlock()
    {
        IDocumentElement element = _documentParser.Parse(
            "====\n    Content in an example block is subject to normal substitutions.\n    ====");

        Assert.NotNull(element);
        Assert.True(element is ExampleBlockElement);
    }

    [Fact]
    public void ParseSideBar()
    {
        IDocumentElement element = _documentParser.Parse(
            "****\n    Sidebars contain aside text and are subject to normal substitutions.\n    ****");

        Assert.NotNull(element);
        Assert.True(element is ExampleBlockElement);
    }

    [Fact]
    public void ParseIdAttribute()
    {
        IDocumentElement element = _documentParser.Parse(
            "[#id-for-listing-block]");

        Assert.NotNull(element);
        Assert.True(element is AttributeElement);
    }

    [Fact]
    public void ParseListingBlock()
    {
        IDocumentElement element = _documentParser.Parse(
            "----\n    Content in a listing block is subject to verbatim substitutions.\n    Listing block content is commonly used to preserve code input.\n    ----");

        Assert.NotNull(element);
        Assert.True(element is ListingBlockElement);
    }


    [Fact]
    public void ParseTable()
    {
        IDocumentElement element = _documentParser.Parse(
            "|===\n    |Column heading 1 |Column heading 2\n    \n    |Column 1, row 1\n    |Column 2, row 1\n    \n    |Column 1, row 2\n    |Column 2, row 2\n    |===\n    ");

        Assert.NotNull(element);
        Assert.True(element is TableElement);
    }

    [Fact]
    public void ParseQuotation()
    {
        IDocumentElement element = _documentParser.Parse(
            "[quote,firstname lastname,movie title]\n    ____\n    I am a block quote or a prose excerpt.\n    I am subject to normal substitutions.\n    ____");

        Assert.NotNull(element);
        Assert.True(element is QuotationElement);
    }

    [Fact]
    public void ParseQuotation_without_attributes()
    {
        IDocumentElement element = _documentParser.Parse(
            "____\n    I am a block quote or a prose excerpt.\n    I am subject to normal substitutions.\n    ____");

        Assert.NotNull(element);
        Assert.True(element is QuotationElement);
    }

    [Fact]
    public void ParseQuotation_style_verse()
    {
        IDocumentElement element = _documentParser.Parse(
            "[verse,firstname lastname,poem title and more]____I am a verse block.Indents and endlines are preserved in verse blocks.____");

        Assert.NotNull(element);
        Assert.True(element is QuotationElement);
    }

    [Fact]
    public void ParseTip()
    {
        IDocumentElement element = _documentParser.Parse(
            "TIP: There are five admonition labels: Tip, Note, Important, Caution and Warning.");

        Assert.NotNull(element);
        // Assert.True(element is IDocumentElement);
    }

    [Fact]
    public void ParseCrossReference()
    {
        IDocumentElement element = _documentParser.Parse(
            "The text at the end of this sentence is cross referenced to <<_third_level_heading,the third level heading>>");

        Assert.NotNull(element);
        Assert.True(element is InlineElement);
    }

    [Fact]
    public void ParseAnchor()
    {
        IDocumentElement element = _documentParser.Parse(
            "This is a link to the https://docs.asciidoctor.org/home/[Asciidoctor documentation].");

        Assert.NotNull(element);
        Assert.True(element is InlineElement);
    }

    [Fact]
    public void ParseQuickReference()
    {
        IDocumentElement element = _documentParser.Parse(
            "This is an attribute reference {url-quickref}[that links this text to the AsciiDoc Syntax Quick Reference].");

        Assert.NotNull(element);
        Assert.True(element is InlineElement);
    }

    //= AsciiDoc Article Title  
    // Firstname Lastname <author@asciidoctor.org>
    // 3.0, July 29, 2022: AsciiDoc article template
    // 
    // :icons: font
}