using System;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Addition;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Blocks.Lists;
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.Factories.Implementations
{
    public class AsciiDocElementFactory : IDocumentElementFactory
    {
        public IDocumentElement Create(Type type, GroupCollection groups)
        {
            IDocumentElement element = type.GetConstructors()[0].Invoke(null) as IDocumentElement;


            if (element is ParagraphElement paragraph)
            {
                paragraph.Paragraph = groups[0].Value;
            }
            else if (element is SectionTitleElement section)
            {
                section.Title = groups[2].Value;
                section.Level = groups[1].Length;
            }
            else if (element is AttributeEntryElement attributeEntry)
            {
                attributeEntry.Name = groups[0].Value;
                try
                {
                    attributeEntry.Value = groups[1].Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else if (element is TitleElement title)
            {
                title.Title = groups[1].Value;
            }
            else if (element.GetType().IsSubclassOf(typeof(ListContainerElement)))
            {
                ListContainerElement listContainer = element as ListContainerElement;

                string value = groups[2].Value;

                listContainer.AddChild(value);
            }
            else if (element is BlockElement block)
            {
            }
            else if (element is ImageReferenceElement imageReference)
            {
                imageReference.ImageReference = groups[1].Value;
                imageReference.AltText = groups[2].Value;
            }
            else if (element is AnchorElement anchor)
            {
                anchor.Reference = groups[1].Value;
                anchor.AltText = groups[2].Value;
            }
            else if (element is CommentElement comment)
            {
                comment.Comment = groups[1].Value;
            }
            else if (element is BoldTextElement boldText)
            {
                boldText.BoldText = groups[1].Value;
            }
            else if (element is ItalicTextElement italicText)
            {
                italicText.ItalicText = groups[1].Value;
            }
            else if (element is CrossReferenceElement crossReference)
            {
                crossReference.CrossReference = groups[1].Value;
                crossReference.AltText = groups[2].Value;
            }
            else if (element is FootNoteElement footNote)
            {
                footNote.Footnote = groups[1].Value;
            }
            else
            {
                throw new NotImplementedException();
            }
            
            return element;
        }
    }
}