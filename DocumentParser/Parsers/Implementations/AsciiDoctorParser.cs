using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using DocumentParser.Elements.Implementations.Blocks.Singles;
using DocumentParser.Elements.Implementations.Inlines;
using DocumentParser.Factories;
using DocumentParser.Factories.Implementations;

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

        public Document LoadFile(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream);
            Document document = new Document();
            document.Body = Parse(streamReader.ReadToEnd());
            return document;
        }

        public List<IDocumentElement> Parse(string context)
        {
            IDocumentElementFactory factory = new AsciiDocElementFactory();
            Stack<BlockElement> blockStack = new Stack<BlockElement>();
            Queue<string> contextQueue = new Queue<string>(context.Split('\n'));
            Queue<IDocumentElement> elements = new Queue<IDocumentElement>();

            Dictionary<string, BlockElement> delimiterDictionary = new Dictionary<string, BlockElement>();

            while (contextQueue.Count > 0)
            {
                string line = contextQueue.Dequeue();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
                Type type = syntax.InstanceType;
                Regex regex = syntax.Pattern;
                GroupCollection groups = regex.Match(line).Groups;

                IDocumentElement element = factory.Create(type, groups);

                Cartridge<IDocumentElement> cartridge = new Cartridge<IDocumentElement>();
                cartridge.Add(element);


                if (element is BlockElement block)
                {
                    string delimiter = groups[0].Value;

                    // 구분자가 일치하는 블록이 있다면 닫는 행
                    if (delimiterDictionary.ContainsKey(delimiter))
                    {
                        BlockElement blockElement = delimiterDictionary[delimiter];
                        cartridge.Remove();
                        // 열린 행까지 스택 pop
                        while (blockStack.Count > 0)
                        {
                            BlockElement popBlock = blockStack.Pop();
                            
                            foreach (KeyValuePair<string, BlockElement> pair in delimiterDictionary)
                            {
                                if (pair.Value.Equals(popBlock))
                                {
                                    delimiterDictionary.Remove(pair.Key);
                                    break;
                                }
                            }

                            if (popBlock.Equals(blockElement))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (blockStack.Count > 0)
                            {
                                BlockElement blockElement = blockStack.Peek();
                                blockElement.AddChild(cartridge.TryRemove());
                                if (blockElement.IsFulled())
                                {
                                    blockStack.Pop();
                                    
                                    foreach (KeyValuePair<string, BlockElement> pair in delimiterDictionary)
                                    {
                                        if (pair.Value.Equals(blockElement))
                                        {
                                            delimiterDictionary.Remove(pair.Key);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                elements.Enqueue(cartridge.TryRemove());
                            }
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine(e);
                        }

                        blockStack.Push(block);
                        delimiterDictionary[delimiter] = block;
                    }
                }
                else if (element is AttributeEntryElement attributeEntry)
                {
                    cartridge.Remove();
                    while (line.EndsWith(" \\"))
                    {
                        line = line.Replace(" \\", "");
                        line += " " + contextQueue.Dequeue();
                    }

                    attributeEntry = factory.Create(type, regex.Match(line).Groups) as AttributeEntryElement;
                    // 헤더에 저장
                }

                try
                {
                    if (blockStack.Count > 0)
                    {
                        BlockElement blockElement = blockStack.Peek();
                        blockElement.AddChild(cartridge.TryRemove());
                        if (blockElement.IsFulled())
                        {
                            blockStack.Pop();
                        }
                    }
                    else
                    {
                        elements.Enqueue(cartridge.TryRemove());
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                }
            }

            return elements.ToList();
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
                    break;
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