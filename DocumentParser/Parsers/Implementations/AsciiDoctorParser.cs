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

            string[] strings = context.Split("\n\n");

            for (int i = 0; i < strings.Length; i++)
            {
                string line = strings[i];
                AsciiDocSyntax syntax = _syntaxAnalyzer.Analyze(line) as AsciiDocSyntax;

                if (syntax.InstanceType.IsSubclassOf(typeof(BlockElement)))
                {
                    StringBuilder builder = new StringBuilder();

                    Group delimiterGroup = syntax.Pattern.Match(line).Groups[2];
                    builder.Append(line.Substring(delimiterGroup.Index));

                    for (i++; i < strings.Length; i++)
                    {
                        string postLine = strings[i];
                        AsciiDocSyntax postSyntax = _syntaxAnalyzer.Analyze(postLine) as AsciiDocSyntax;

                        if (syntax.Pattern.Match(line).Value.Equals(postSyntax.Pattern.Match(strings[i]).Value))
                        {
                            break;
                        }
                        else
                        {
                            builder.Append(strings[i]);
                        }
                    }

                    BlockElement element = syntax.InstanceType.GetConstructors()[0].Invoke(null)
                        as BlockElement;

                    documentElements.Add(element);

                    List<IDocumentElement> list = Parse(builder.ToString());

                    element.Children = list;
                }
                else
                {
                    IDocumentElement element = syntax.InstanceType.GetConstructors()[0].Invoke(new object[] { strings[i] })
                        as IDocumentElement;
                    documentElements.Add(element);
                }
            }


            return documentElements;
        }
    }
}