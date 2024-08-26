using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentParser.Domain;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.DocumentSyntaxes
{
    public class AsciiDocSyntax : IEnumerable<AsciiDocSyntax>, IDocumentSyntax
    {
        public AsciiDocSyntax()
        {
        }


        public AsciiDocSyntax(string index, string pattern, Type instanceType)
        {
            Index = index;
            Pattern = new Regex(pattern);
            InstanceType = instanceType;
        }


        public AsciiDocSyntax(string index, Regex pattern, Type instanceType)
        {
            Index = index;
            Pattern = pattern;
            InstanceType = instanceType;
        }

        public string Index { get; }
        public Regex Pattern { get; }
        public Type InstanceType { get; }

        public IEnumerator<AsciiDocSyntax> GetEnumerator()
        {
            yield return new AsciiDocSyntax("=", "=", typeof(SectionTitleElement));
            yield return new AsciiDocSyntax(":", ":", typeof(AttributeEntryElement));

            // Block, Structural containers

            // yield return new AsciiDocSyntax("/", "/{4,}\n.*|^[^\n]*$", typeof(CommentBlockElement));
            // yield return new AsciiDocSyntax("=", "={4,}\n.*|^[^\n]*$", typeof(ExampleBlockElement));
            // yield return new AsciiDocSyntax("-", "-{4,}\n.*|^[^\n]*$", typeof(ListingBlockElement));
            // yield return new AsciiDocSyntax(".", ".{4,}\n.*|^[^\n]*$", typeof(LiteralBlockElement));
            // yield return new AsciiDocSyntax("-", "-{2,}\n.*|^[^\n]*$", typeof(OpenBlockElement));
            yield return new AsciiDocSyntax("*", "(\\*{4,})+(\n.*|^[^\n]*$)", typeof(SideBarBlockElement));
            //
            // yield return new AsciiDocSyntax("|", "\\|={3,}\n.*|^[^\n]*$", typeof(TableBlockElement));
            // yield return new AsciiDocSyntax(",", ",={3,}\n.*|^[^\n]*$", typeof(TableBlockElement));
            // yield return new AsciiDocSyntax(":", ":={3,}\n.*|^[^\n]*$", typeof(TableBlockElement));
            // yield return new AsciiDocSyntax("!", "!={3,}\n.*|^[^\n]*$", typeof(TableBlockElement));
            //
            // yield return new AsciiDocSyntax("+", "+{4,}\n.*|^[^\n]*$", typeof(PassBlockElement));
            // yield return new AsciiDocSyntax("_", "_{4,}\n.*|^[^\n]*$", typeof(QuotationBlockElement));


            //
            // yield return new AsciiDocSyntax(".", "^\\.[^ .]", typeof(TitleElement));
            // yield return new AsciiDocSyntax(".", "\\.", typeof(OrderedListElement));
            //
            // yield return new AsciiDocSyntax("*", "\\*\\*\\*\\*$", typeof(SideBarElement));
            // yield return new AsciiDocSyntax("*", "\\*", typeof(UnOrderedListElement));
            //
            // yield return new AsciiDocSyntax("-", "----$", typeof(ListingBlockElement));
            // yield return new AsciiDocSyntax("-", "-", typeof(UnOrderedListElement));
            //
            // yield return new AsciiDocSyntax("i", "^image::", typeof(ImageReferenceElement));
            //
            // yield return new AsciiDocSyntax("|", "\\|===$", typeof(TableElement));
            //
            // yield return new AsciiDocSyntax("[", "\\[.+]$", typeof(AttributeElement));
            //
            // yield return new AsciiDocSyntax("_", "____$", typeof(QuotationElement));
            //
            // yield return new AsciiDocSyntax("/", "//", typeof(CommentElement));
            //
            //
            // yield return new AsciiDocSyntax(string.Empty, "<<+.+>>", typeof(CrossReferenceElement));
            //
            // yield return new AsciiDocSyntax(string.Empty, "(\\S+://)(.+)(\\[(.*)])", typeof(AnchorElement));
            // yield return new AsciiDocSyntax(string.Empty, "(\\S+://[^ ]+)", typeof(AnchorElement));
            //
            // yield return new AsciiDocSyntax(string.Empty, "\\*(.*)\\*", typeof(BoldTextElement));
            // yield return new AsciiDocSyntax(string.Empty, "_(.*)_", typeof(ItalicTextElement));
            //
            // yield return new AsciiDocSyntax(string.Empty, "footnote:\\[(.*)]", typeof(FootNoteElement));

            yield return new AsciiDocSyntax(string.Empty, "", typeof(ParagraphElement));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}