﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentParser.Elements.Implementations.Addition;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Blocks.Lists;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.DocumentSyntaxes.implementations
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
            yield return new AsciiDocSyntax("=", new Regex("^(={1,}) +(.*)"), typeof(SectionTitleElement));
            yield return new AsciiDocSyntax(":", new Regex("^:([^ ].+):(.*)"), typeof(AttributeEntryElement));

            // Block, Structural containers

            yield return new AsciiDocSyntax("/", new Regex("^(/{4,})+$|(\n.*)"), typeof(CommentBlockElement));
            yield return new AsciiDocSyntax("=", new Regex("^(={4,})+$|(\n.*)"), typeof(ExampleBlockElement));
            yield return new AsciiDocSyntax("-", new Regex("^(-{4,})+$|(\n.*)"), typeof(ListingBlockElement));
            yield return new AsciiDocSyntax("-", new Regex("^(-{2,})+$|(\n.*)"), typeof(OpenBlockElement));
            yield return new AsciiDocSyntax("_", new Regex("^(_{4,})+$|(\n.*)"), typeof(QuotationBlockElement));
            yield return new AsciiDocSyntax(".", new Regex("^(\\.{4,})+$|(\n.*)"), typeof(LiteralBlockElement));
            yield return new AsciiDocSyntax("*", new Regex("^(\\*{4,})+$|(\n.*)"), typeof(SideBarBlockElement));
            yield return new AsciiDocSyntax("+", new Regex("^(\\+{2,})+$|(\n.*)"), typeof(PassBlockElement));

            // Block, Table
            yield return new AsciiDocSyntax("|", new Regex("^(\\|={3,})+$|(\n.*)"), typeof(TableBlockElement));
            yield return new AsciiDocSyntax(",", new Regex("^(,={3,})+$|(\n.*)"), typeof(TableBlockElement));
            yield return new AsciiDocSyntax(":", new Regex("^(:={3,})+$|(\n.*)"), typeof(TableBlockElement));
            yield return new AsciiDocSyntax("!", new Regex("^(!={3,})+$|(\n.*)"), typeof(TableBlockElement));


            yield return new AsciiDocSyntax(".", new Regex("^\\.([^ .]+.+)"), typeof(TitleElement));
            yield return new AsciiDocSyntax("[", new Regex("^\\[+(.*)+]"), typeof(SpecialBlockElement));

            yield return new AsciiDocSyntax(".", new Regex("^(\\.{1,})+ (.*)"), typeof(OrderedListContainerElement));
            yield return new AsciiDocSyntax("*", new Regex("^(\\*{1,})+ (.*)"), typeof(UnOrderedListContainerElement));
            yield return new AsciiDocSyntax("-", new Regex("^(\\-{1,})+ (.*)"), typeof(UnOrderedListContainerElement));

            yield return new AsciiDocSyntax("/", new Regex("^//(.*)"), typeof(CommentElement));
            
            yield return new AsciiDocSyntax("i", new Regex("^image::([^\\[]+)(?:\\[(.*)?\\])?$"),
                typeof(ImageReferenceElement));
            yield return new AsciiDocSyntax("i", new Regex("image:([^\\[]+)(?:\\[(.+)\\])?$"),
                typeof(ImageReferenceElement));

            yield return new AsciiDocSyntax(string.Empty, new Regex("<<(.+),(.+)?>>"), typeof(CrossReferenceElement));
            yield return new AsciiDocSyntax(string.Empty, new Regex("(\\S+://.+)(?:\\[(.+)\\])"),
                typeof(AnchorElement));
            //
            yield return new AsciiDocSyntax(string.Empty, "\\*(.*)\\*", typeof(BoldTextElement));
            yield return new AsciiDocSyntax(string.Empty, "_(.*)_", typeof(ItalicTextElement));
            yield return new AsciiDocSyntax(string.Empty, "footnote:\\[(.*)]", typeof(FootNoteElement));

            yield return new AsciiDocSyntax(string.Empty, new Regex("(.*)?"), typeof(ParagraphElement));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}