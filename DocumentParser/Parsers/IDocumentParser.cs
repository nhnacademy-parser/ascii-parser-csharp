using System;
using System.IO;
using Parser.Domain;

namespace parser.Parsers
{
    public interface IDocumentParser
    {
        Document LoadFile(string filePath);
        Document LoadFile(File file);

        IDocumentParser Parse(Document document);
    }
}

