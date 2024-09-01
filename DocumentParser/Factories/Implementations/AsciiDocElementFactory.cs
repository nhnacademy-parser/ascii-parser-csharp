using System;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Addition;
using DocumentParser.Elements.Implementations.Blocks;
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
                section.Title = groups[1].Value;
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
            else if (element is BlockElement block)
            {
                
            }
            else
            {
                throw new NotImplementedException();
            }

            return element;
        }
    }
}