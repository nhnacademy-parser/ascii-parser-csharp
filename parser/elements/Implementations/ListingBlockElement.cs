
namespace Parser.Elements.Implementations
{
    public class ListingBlockElement: DocsElement
    {
        public ListingBlockElement(string example) : base(example)
        {
        }

        public string ListingBlock
        {
            get { return Value.ToString(); }
        }
    }
}