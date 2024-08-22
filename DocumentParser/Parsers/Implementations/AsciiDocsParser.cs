using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentParser.Analyzers.Implementations;
using DocumentParser.DocumentSyntaxes;
using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Parsers.Implementations
{
    public class AsciiDocsParser : IDocumentParser
    {
        private readonly AsciiDocsAnalyzer _analyzer;

        public AsciiDocsParser()
        {
            _analyzer = new AsciiDocsAnalyzer();
            _analyzer.Init();
        }

        public Document LoadFile(string filePath)
        {
            return LoadFile(File.OpenRead(filePath));
        }

        public Document LoadFile(FileStream file)
        {
            List<DocsElement> documentElements = new List<DocsElement>();
            StreamReader inputStreamReader = new StreamReader(file);

            while (!inputStreamReader.EndOfStream)
            {
                string str = inputStreamReader.ReadLine();

                DocsElement element = Parse(str) as DocsElement;

                documentElements.Add(element);
            }

            return Assemble(documentElements);
        }

        private Document Assemble(List<DocsElement> docsElements)
        {
            Document document = new Document();

            DocsElement head = null;

            Cartridge<AttributeElement> attributeElement = new Cartridge<AttributeElement>();
            foreach (DocsElement docsElement in docsElements)
            {
                Cartridge<DocsElement> element = new Cartridge<DocsElement>();
                element.TryAdd(docsElement);

                if (docsElement is AttributeElement)
                {
                    try
                    {
                        document.Append(attributeElement.TryRemove());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                    }

                    attributeElement.TryAdd(element.TryRemove() as AttributeElement);
                }
                else
                {
                    try
                    {
                        docsElement.AddAttribute(attributeElement.TryRemove());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                    }

                    try
                    {
                        while (head != null)
                        {
                            bool flag = false;

                            try
                            {
                                flag = EndOfContainer(head, docsElement);
                            }
                            catch (ContainerCloseElementException e)
                            {
                                Console.WriteLine(e);
                                head = head.Parent;
                                throw new ContainerCloseElementException();
                            }

                            if (flag)
                            {
                                head = head.Parent;
                            }
                            else
                            {
                                head.AddChild(element.TryRemove());
                                break;
                            }
                        }
                    }
                    catch (ContainerCloseElementException e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }

                    if (docsElement.IsContainer)
                    {
                        docsElement.Parent = head;
                        head = docsElement;
                    }

                    try
                    {
                        document.Append(element.TryRemove());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return document;
        }

        public IDocumentElement Parse(string context)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                return new PlainTextElement("");
            }

            IDocumentSyntax syntax = _analyzer.Analyze(context);

            if (syntax is AsciiDocSyntax asciiDocSyntax)
            {
                Type type = asciiDocSyntax.InstanceType;
                Regex pattern = asciiDocSyntax.Pattern;

                if (type == typeof(HeadingElement))
                {
                    return new HeadingElement(ParseLine(pattern.Replace(context, (match) => "")),
                        pattern.Matches(context).Count);
                }
                else if (type == typeof(ExampleBlockElement))
                {
                    return new ExampleBlockElement(context);
                }
                else if (type == typeof(TitleElement))
                {
                    return new TitleElement(context[1..]);
                }
                else if (type == typeof(OrderedListElement))
                {
                    return new OrderedListElement(ParseLine(pattern.Replace(context, match => "")),
                        pattern.Matches(context).Count);
                }
                else if (type == typeof(UnOrderedListElement))
                {
                    return new UnOrderedListElement(ParseLine(pattern.Replace(context, match => "")),
                        pattern.Matches(context).Count);
                }
                else if (type == typeof(SideBarElement))
                {
                    return new SideBarElement(ParseLine(pattern.Replace(context, match => "")));
                }
                else if (type == typeof(ListingBlockElement))
                {
                    return new ListingBlockElement(ParseLine(pattern.Replace(context, match => "")));
                }
                else if (type == typeof(ImageElement))
                {
                    String href = pattern.Replace(context, "");

                    Regex altTextRegex = new Regex("\\[.+]");
                    Match match = altTextRegex.Match(href);

                    if (match.Success)
                    {
                        String alt = match.Value;

                        href = altTextRegex.Replace(href, "");
                        return new ImageElement(href, alt.Substring(1, alt.Length - 2));
                    }
                }
                else if (type == typeof(TableElement))
                {
                    return new TableElement("");
                }
                else if (type == typeof(AttributeElement))
                {
                    // Quotation Attribute
                    Regex attributeRegex = new Regex("\\[.+]$");
                    Match match = attributeRegex.Match(context);

                    if (match.Success)
                    {
                        string attributes = match.Value;
                        string[] temp = new string[3];
                        string[] copy = attributes.Substring(1, attributes.Length - 2).Split(",");

                        Array.Copy(copy, 0, temp, 0, Math.Min(copy.Length, 3));

                        Regex regex = new Regex("^#");
                        Match match1 = regex.Match(temp[0]);

                        if (match1.Success)
                        {
                            // Attribute 에 대한 재정의 필요
                            return new AttributeElement();
                        }

                        return new AttributeElement();
                    }
                }
                else if (type == typeof(QuotationElement))
                {
                    return new QuotationElement(ParseLine(pattern.Replace(context, match => "")));
                }
                else if (type == typeof(CommentElement))
                {
                    return new CommentElement(pattern.Replace(context, ""));
                }
                else if (type == typeof(VariableElement))
                {
                }
            }

            return new InlineElement(ParseLine(context));
        }

        private IDocumentElement ParseLine(string context)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                return new PlainTextElement("");
            }

            IDocumentSyntax syntax = _analyzer.Analyze(context);

            if (syntax is AsciiDocSyntax asciiDocSyntax)
            {
                Type type = asciiDocSyntax.InstanceType;
                Regex pattern = asciiDocSyntax.Pattern;
                Match match = pattern.Match(context);

                if (Equals(type, typeof(CrossReferenceElement)))
                {
                    if (match.Success)
                    {
                        string temp = new Regex("[<>]").Replace(match.Value, "");
                        string[] strings = temp.Split(",");


                        IDocumentElement pre = ParseLine(context[..match.Index]);
                        IDocumentElement mid = new CrossReferenceElement(strings[0], strings[1]);
                        IDocumentElement post = ParseLine(context[(match.Index + match.Length)..]);

                        pre.Append(mid).Append(post);

                        return pre;
                    }
                }
                else if (Equals(type, typeof(AnchorElement)))
                {
                    string href = null;
                    string altText = null;

                    try
                    {
                        href = match.Groups[1].Value;
                        href += match.Groups[2].Value;

                        altText = match.Groups[4].Value;
                    }
                    catch (Exception ignore)
                    {
                    }

                    IDocumentElement pre = ParseLine(context[..match.Index]);
                    IDocumentElement mid = new AnchorElement(href, altText);
                    IDocumentElement post = ParseLine(context[(match.Index + match.Length)..]);

                    pre.Append(mid).Append(post);

                    return pre;
                }
                else if (Equals(type, typeof(BoldTextElement)))
                {
                    IDocumentElement pre = ParseLine(context[..match.Index]);
                    IDocumentElement mid = new BoldTextElement(ParseLine(match.Groups[1].Value));
                    IDocumentElement post = ParseLine(context[(match.Index + match.Length)..]);

                    pre.Append(mid).Append(post);

                    return pre;
                }
                else if (Equals(type, typeof(ItalicTextElement)))
                {
                    IDocumentElement pre = ParseLine(context[..match.Index]);
                    IDocumentElement mid = new ItalicTextElement(ParseLine(match.Groups[1].Value));
                    IDocumentElement post = ParseLine(context[(match.Index + match.Length)..]);

                    pre.Append(mid).Append(post);

                    return pre;
                }
                else if (Equals(type, typeof(FootNoteElement)))
                {
                    IDocumentElement pre = ParseLine(context[..match.Index]);
                    IDocumentElement mid = new FootNoteElement(ParseLine(match.Groups[1].Value));
                    IDocumentElement post = ParseLine(context[(match.Index + match.Length)..]);

                    pre.Append(mid).Append(post);

                    return pre;
                }
            }

            return new PlainTextElement(context);
        }

        private bool EndOfContainer(IDocumentElement left, IDocumentElement right)
        {
            if (left is UnOrderedListElement leftUl)
            {
                if (right is UnOrderedListElement rightUl)
                {
                    return rightUl.Level <= leftUl.Level;
                }

                return string.IsNullOrWhiteSpace(right.ToString());
            }
            else if (left is OrderedListElement leftOl)
            {
                if (right is OrderedListElement rightOl)
                {
                    return rightOl.Level <= leftOl.Level;
                }

                return string.IsNullOrWhiteSpace(right.ToString());
            }
            else if (left.GetType() == right.GetType())
            {
                throw new ContainerCloseElementException();
            }

            return false;
        }
    }

    internal class ContainerCloseElementException : SystemException
    {
    }
}