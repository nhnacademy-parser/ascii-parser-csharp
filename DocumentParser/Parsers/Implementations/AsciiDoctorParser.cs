using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DocumentParser.Analyzers;
using DocumentParser.Analyzers.Implementations;
using DocumentParser.DocumentSyntaxes;
using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Addition;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.Parsers.Implementations
{
    public class AsciiDoctorParser : IDocumentParser
    {
        private IDocumentSyntaxAnalyzer _syntaxAnalyzer;

        public AsciiDoctorParser()
        {
            var asciiDocsAnalyzer = new AsciiDocsAnalyzer();
            asciiDocsAnalyzer.Init();

            _syntaxAnalyzer = asciiDocsAnalyzer;
        }


        public Document LoadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Document LoadFile(Stream file)
        {
            throw new NotImplementedException();
        }

        public List<IDocumentElement> Parse(string context)
        {
            List<IDocumentElement> documentElements = new List<IDocumentElement>();

            Queue<string> lineQueue = new Queue<string>(context.Split('\n'));
            Cartridge<TitleElement> titleCartridge = new Cartridge<TitleElement>();

            StringBuilder bodyBuilder = new StringBuilder();

            while (lineQueue.Count > 0)
            {
                string line = lineQueue.Peek();
                if (line.Trim().Length == 0 && bodyBuilder.Length != 0)
                {
                    IDocumentElement element = new ParagraphElement(bodyBuilder.ToString());
                    bodyBuilder.Clear();
                    documentElements.Add(element);
                }

                AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
                Type type = syntax.InstanceType;

                if (syntax.InstanceType == typeof(SpecialBlockElement))
                {
                    documentElements.Add(ParseSpecialBlock(lineQueue));
                }
                else if (type.IsSubclassOf(typeof(BlockElement)))
                {
                    documentElements.Add(ParseBlockElement(lineQueue));
                }
                else if (syntax.InstanceType == typeof(SectionTitleElement))
                {
                    line = lineQueue.Dequeue();
                    Group delimiterGroup = syntax.Pattern.Match(line).Groups[0];
                    string sectionTitle = line.Substring(delimiterGroup.Index + delimiterGroup.Length);
                    IDocumentElement element =
                        syntax.InstanceType.GetConstructors()[0].Invoke(new object[]
                            { sectionTitle }) as IDocumentElement;
                    documentElements.Add(element);
                }
                else if (syntax.InstanceType == typeof(AttributeEntryElement))
                {
                    line = lineQueue.Dequeue();
                    StringBuilder attributeBuilder = new StringBuilder();
                    attributeBuilder.Append(line);
                    while (attributeBuilder.ToString().Trim().EndsWith(" \\"))
                    {
                        attributeBuilder.Append(lineQueue.Dequeue());
                    }

                    IDocumentElement element =
                        syntax.InstanceType.GetConstructors()[0].Invoke(new object[]
                            { attributeBuilder.ToString() }) as IDocumentElement;
                    documentElements.Add(element);
                }
                else if (syntax.InstanceType == typeof(TitleElement))
                {
                    line = lineQueue.Dequeue();
                    TitleElement element =
                        syntax.InstanceType.GetConstructors()[0].Invoke(new object[]
                            { line }) as TitleElement;
                    // titleCartridge.TryAdd(element);
                }
                else
                {
                    bodyBuilder.Append(lineQueue.Dequeue());
                }
            }

            if (bodyBuilder.Length > 0)
            {
                IDocumentElement element = new ParagraphElement(bodyBuilder.ToString());
                documentElements.Add(element);
            }

            return documentElements;
        }

        private IDocumentElement ParseBlockElement(Queue<string> lineQueue)
        {
            string line = lineQueue.Dequeue();
            AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
            Type type = syntax.InstanceType;
            BlockElement block = type.GetConstructors()[0].Invoke(null) as BlockElement;

            StringBuilder blockContent = new StringBuilder();

            Group preDelimiterGroup = syntax.Pattern.Match(line).Groups[0];

            while (lineQueue.Count > 0)
            {
                line = lineQueue.Dequeue();

                AsciiDocSyntax postSyntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;

                if (postSyntax.InstanceType.IsSubclassOf(typeof(BlockElement)))
                {
                    Group postDelimiterGroup
                        = syntax.Pattern.Match(line).Groups[0];

                    if (preDelimiterGroup.Value.Equals(postDelimiterGroup.Value))
                    {
                        Parse(blockContent.ToString()).ForEach(e => block.Children.Add(e));
                        blockContent.Clear();
                        break;
                    }
                }

                blockContent.Append(line).Append('\n');
            }

            if (blockContent.Length > 0)
            {
                List<IDocumentElement> elements = Parse(blockContent.ToString());
                blockContent.Clear();
                elements.ForEach(e => block.Children.Add(e));
            }

            return block;
        }

        private IDocumentElement ParseSpecialBlock(Queue<string> lineQueue)
        {
            string line = lineQueue.Dequeue(); // block type
            Console.WriteLine("Special block parsing Start, block type is " + line);
            AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
            Type type = syntax.InstanceType;
            BlockElement block = type.GetConstructors()[0].Invoke(null) as BlockElement;

            StringBuilder contentBuilder = new StringBuilder();

            while (lineQueue.Count > 0)
            {
                string subLine = lineQueue.Peek();
                syntax = _syntaxAnalyzer.Analyze(subLine) as AsciiDocSyntax;

                if (block.Children.Count > 0)
                {
                    break;
                }
                else if (syntax.InstanceType == typeof(TitleElement))
                {
                    TitleElement title =
                        syntax.InstanceType.GetConstructors()[0].Invoke(new object[]
                            { lineQueue.Dequeue() }) as TitleElement;
                    // titleCartridge.TryAdd(title);
                    continue;
                }
                else if (syntax.InstanceType == typeof(SpecialBlockElement))
                {
                    Parse(contentBuilder.ToString()).ForEach(e => block.Children.Add(e));
                    break;
                }
                else if (syntax.InstanceType.IsSubclassOf(typeof(BlockElement)))
                {
                    block.Children.Add(ParseBlockElement(lineQueue));
                }
                else
                {
                    contentBuilder.Append(lineQueue.Dequeue());
                }
            }

            return block;
        }
    }
}