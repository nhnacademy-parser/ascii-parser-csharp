using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Blocks.Lists;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.Visitors
{
    public interface IDocumentVisitor
    {
        string Visit(IDocumentElement element);
        string Visit(LineElement element);
        string Visit(InlineElement element);
        string Visit(BoldTextElement element);
        string Visit(ItalicTextElement element);
        string Visit(ParagraphElement element);
        string Visit(SectionTitleElement element);
        string Visit(ListElement element);
        string Visit(ListContainerElement element);
        string Visit(TitleElement element);
        string Visit(BlockElement element);
        string Visit(QuotationBlockElement element);
        string Visit(ImageReferenceElement element);
        string Visit(AnchorElement element);
        string Visit(CrossReferenceElement element);
        string Visit(CommentElement element);
        
    }
}