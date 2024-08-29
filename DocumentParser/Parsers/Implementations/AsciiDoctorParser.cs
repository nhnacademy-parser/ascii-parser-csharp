using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DocumentParser.Analyzers;
using DocumentParser.Analyzers.Implementations;
using DocumentParser.DocumentSyntaxes;
using DocumentParser.DocumentSyntaxes.implementations;
using DocumentParser.Domain;
using DocumentParser.Domains.Trees;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Addition;
using DocumentParser.Elements.Implementations.Blocks;
using DocumentParser.Elements.Implementations.Blocks.Lists;
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

            while (lineQueue.Count > 0)
            {
                string line = lineQueue.Peek();

                if (string.IsNullOrWhiteSpace(line))
                {
                    lineQueue.Dequeue();
                    continue;
                }

                AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
                Type type = syntax.InstanceType;

                if (syntax.InstanceType == typeof(SpecialBlockElement))
                {
                    documentElements.Add(ParseSpecialBlock(lineQueue));
                }
                else if (syntax.InstanceType.IsSubclassOf(typeof(ListContainerElement)))
                {
                    documentElements.Add(ParseListElement(lineQueue));
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
                    // documentElements.Add(element);
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
                    documentElements.Add(new ParagraphElement(lineQueue.Dequeue()));
                }
            }

            return documentElements;
        }

        private IDocumentElement ParseListElement(Queue<string> lineQueue)
        {
            Dictionary<IDocumentElement, string> delimiterMap = new Dictionary<IDocumentElement, string>();
            Cartridge<String> breakMethod = new Cartridge<string>();

            ListContainerElement root;
            {
                string line = lineQueue.Dequeue();
                AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
                Type type = syntax.InstanceType;
                GroupCollection groups = syntax.Pattern.Match(line).Groups;
                string delimiter = groups[1].Value;
                string content = groups[2].Value;
                root = type.GetConstructors()[0].Invoke(
                    new object[] { content }) as ListContainerElement;
                delimiterMap[root] = delimiter;
            }

            ListContainerElement beforeElement = root;

            while (lineQueue.Count > 0)
            {
                string line = lineQueue.Peek();
                AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
                Type type = syntax.InstanceType;

                if (string.IsNullOrWhiteSpace(line))
                {
                    breakMethod.Add(lineQueue.Dequeue());
                    continue;
                }

                if (type == typeof(ParagraphElement))
                {
                    if (breakMethod.IsFilled)
                    {
                        break;
                    }

                    lineQueue.Dequeue();
                    // beforeElement.Value += nextLine; 바로 윗 줄이 빈칸이 아니면 이전 요소에 옆으로 이어 붙인다.
                }
                else if (!type.IsSubclassOf(typeof(ListContainerElement)))
                {
                    break;
                }
                else
                {
                    GroupCollection groups = syntax.Pattern.Match(lineQueue.Dequeue()).Groups;
                    string delimiter = groups[1].Value;
                    string content = groups[2].Value;

                    ListContainerElement e =
                        type.GetConstructors()[0].Invoke(new object[] { content }) as ListContainerElement;
                    delimiterMap.Add(e, delimiter);

                    ListContainerElement current = beforeElement;

                    while (true)
                    {
                        if (current.Parent == null)
                        {
                            beforeElement.AddChild(e);
                            break;
                        }

                        if (delimiter.Equals(delimiterMap[current]))
                        {
                            current.AddChild(content);
                            break;
                        }

                        current = current.Parent as ListContainerElement;
                    }

                    beforeElement = e;
                }

                breakMethod.Remove();
            }

            return root;
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
                        Parse(blockContent.ToString()).ForEach(e => block.AddChild(e));
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
                elements.ForEach(e => block.AddChild(e));
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
                    Parse(contentBuilder.ToString()).ForEach(e => block.AddChild(e));
                    break;
                }
                else if (syntax.InstanceType.IsSubclassOf(typeof(BlockElement)))
                {
                    block.AddChild(ParseBlockElement(lineQueue));
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