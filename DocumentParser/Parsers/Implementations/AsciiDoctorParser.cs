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
                Cartridge<IDocumentElement> cartridge = new Cartridge<IDocumentElement>();

                IDocumentElement element = factory.Create(type, groups);
                cartridge.Add(element);

                if (type.IsSubclassOf(typeof(LineElement)))
                {
                    cartridge.Remove();
                    InlineElement inline = new InlineElement();
                    inline.AddChild(ParseLineElement(line));
                    cartridge.Add(inline);
                }
                else if (element is BlockElement block)
                {
                    if (block.GetType().IsSubclassOf(typeof(ListContainerElement)))
                    {
                        Dictionary<BlockElement, string> delimiterMap = new Dictionary<BlockElement, string>();
                        Cartridge<String> breakMethod = new Cartridge<string>();
                        ListContainerElement beforeElement = block as ListContainerElement;
                        delimiterMap.Add(block, groups[1].Value);

                        while (contextQueue.Count > 0)
                        {
                            string subLine = contextQueue.Peek();
                            AsciiDocSyntax subSyntax = _syntaxAnalyzer.Analyze(subLine) as AsciiDocSyntax;
                            
                            if (string.IsNullOrWhiteSpace(subLine))
                            {
                                breakMethod.Add(contextQueue.Dequeue());
                                continue;
                            }

                            Type subType = subSyntax.InstanceType;
                            if (subType == typeof(ParagraphElement))
                            {
                                if (breakMethod.IsFilled)
                                {
                                    break;
                                }

                                contextQueue.Dequeue();
                                ((beforeElement.Children.Last() as ListElement).Value as ParagraphElement).Paragraph +=
                                    subLine;
                            }
                            else if (!subType.IsSubclassOf(typeof(ListContainerElement)))
                            {
                                break;
                            }
                            else
                            {
                                GroupCollection subGroups = subSyntax.Pattern.Match(contextQueue.Dequeue()).Groups;
                                string delimiter = subGroups[1].Value;
                                string content = subGroups[2].Value;

                                ListContainerElement subElement =
                                    factory.Create(subType, subGroups) as ListContainerElement;

                                delimiterMap.Add(subElement, delimiter);

                                ListContainerElement current = beforeElement;

                                while (true)
                                {
                                    if (delimiter.Equals(delimiterMap[current]))
                                    {
                                        current.AddChild(content);
                                        subElement = current;
                                        break;
                                    }

                                    if (current.Parent == null)
                                    {
                                        beforeElement.AddChild(subElement);
                                        break;
                                    }

                                    current = current.Parent as ListContainerElement;
                                }

                                beforeElement = subElement;
                            }

                            breakMethod.Remove();
                        }
                    }
                    else
                    {
                        string delimiter = groups[0].Value;

                        if (delimiterDictionary.ContainsKey(delimiter)) // 구분자가 일치하는 블록이 있다면 닫는 행
                        {
                            BlockElement blockElement = delimiterDictionary[delimiter];
                            cartridge.Remove();
                            // 열린 행까지 스택 pop
                            while (blockStack.Count > 0)
                            {
                                BlockElement popBlock = blockStack.Pop();
                                RemoveDictionary(delimiterDictionary, blockElement);

                                if (popBlock.Equals(blockElement))
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            NewMethod(blockStack, cartridge, delimiterDictionary, elements);

                            blockStack.Push(block);
                            delimiterDictionary[delimiter] = block;
                        }
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

                NewMethod(blockStack, cartridge, delimiterDictionary, elements);
            }

            return elements.ToList();
        }

        private LineElement ParseLineElement(string line)
        {
            IDocumentElementFactory factory = new AsciiDocElementFactory();
            AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;
            Type type = syntax.InstanceType;
            Regex regex = syntax.Pattern;
            Match match = regex.Match(line);
            GroupCollection groups = regex.Match(line).Groups;

            IDocumentElement element = factory.Create(type, groups) ;
            string pre = line[..match.Index];
            string post = line[(match.Index + match.Length)..];

            LineElement head = element as LineElement;
            
            if (!string.IsNullOrWhiteSpace(pre))
            {
                head = ParseLineElement(pre).Append(head) as LineElement;
            }

            if (!string.IsNullOrWhiteSpace(post))
            {
                head.Append(ParseLineElement(post));
            }

            return head;
        }

        private void NewMethod(Stack<BlockElement> blockStack, Cartridge<IDocumentElement> cartridge,
            Dictionary<string, BlockElement> delimiterDictionary, Queue<IDocumentElement> elements)
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

                        RemoveDictionary(delimiterDictionary, blockElement);
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

        private void RemoveDictionary(Dictionary<string, BlockElement> delimiterDictionary,
            BlockElement blockElement)
        {
            foreach (var pair in delimiterDictionary.Where(pair => pair.Value.Equals(blockElement)))
            {
                delimiterDictionary.Remove(pair.Key);
                break;
            }
        }
    }
}