using parser.elements.Implementations;

namespace parser.visitors
{
    public interface IDocumentVisitor
    {
        string Visit(DocsElement element);

        string Visit(HeadingElement element);

        string Visit(ExampleElement element);

        string Visit(TitleElement element);

        string Visit(ListElement element);

        string Visit(UnOrderedListElement element);

        string Visit(SideBarElement element);

        string Visit(ImageElement element);

        string Visit(TableElement element);

        string Visit(QuotationElement element);

        string Visit(AttributeElement element);

        string Visit(CommentElement element);

        string Visit(CrossReferenceElement element);

        string Visit(AnchorElement element);

        string Visit(ItalicTextElement element);

        string Visit(BoldTextElement element);

        string Visit(FootNoteElement element);
    }
}