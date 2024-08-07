using System;
using System.IO;
using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Parsers;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Parsers.Implementations
{
	public class AsciiDocsParser:IDocumentParser
	{
		public AsciiDocsParser()
		{
		}

        public Document LoadFile(string filePath)
        {
            return LoadFile(File.OpenRead(filePath));
        }

        public Document LoadFile(FileStream file)
        {
            Document document = new Document();

            DocsElement headElement = new DocsElement();
            StreamReader inputStreamReader = new StreamReader(file);

            while(!inputStreamReader.EndOfStream)
            { 
                string str = inputStreamReader.ReadLine();

                DocsElement element = Parse(str);

                if (headElement.EndOfContaier(element))
                {
                    headElement = element.Parent;
                }

                headElement.AddChild(element);

                if (element.IsContainer)
                { 
                    element.Parent = headElement;
                    headElement = element;
                }
            }

            return document;
        }

        public DocsElement Parse(string document)
        {
            DocsElement docsElement = new DocsElement();

            

            return docsElement;
        }
    }
}

