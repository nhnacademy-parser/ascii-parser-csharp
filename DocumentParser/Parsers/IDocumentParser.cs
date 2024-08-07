using System;
using System.IO;
using DocumentParser.Domain;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Parsers
{
    public interface IDocumentParser
    {
        Document LoadFile(string filePath);
        Document LoadFile(FileStream file);

        DocsElement Parse(string document);
    }
}

