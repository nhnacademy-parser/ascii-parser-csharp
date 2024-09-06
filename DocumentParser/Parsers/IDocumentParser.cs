using System;
using System.Collections.Generic;
using System.IO;
using DocumentParser.Domain;
using DocumentParser.Elements;

namespace DocumentParser.Parsers
{
    public interface IDocumentParser
    {
        
        
        Document LoadFile(string filePath);
        Document LoadFile(Stream file);

        List<IDocumentElement> Parse(string context);
    }
}

