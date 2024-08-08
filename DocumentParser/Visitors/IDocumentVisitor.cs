using DocumentParser.Elements.Implementations;

namespace DocumentParser.Visitors
{
    public interface IDocumentVisitor
    {
        string Visit(AnchorElement element);

        string Visit(AttributeElement element);

        string Visit(BoldTextElement element);

        string Visit(CommentElement element);

        string Visit(CrossReferenceElement element);

        string Visit(DocsElement element);

        string Visit(ExampleBlockElement element);

        string Visit(FootNoteElement element);

        string Visit(HeadingElement element);

        string Visit(ImageElement element);

        string Visit(ItalicTextElement element);

        string Visit(IdAttributeElement element);

        string Visit(ListingBlockElement element);

        string Visit(LineElement element);

        string Visit(OrderedListElement element);

        string Visit(QuotationElement element);

        string Visit(SideBarElement element);

        string Visit(TableElement element);

        string Visit(TitleElement element);

        string Visit(UnOrderedListElement element);

    }
}