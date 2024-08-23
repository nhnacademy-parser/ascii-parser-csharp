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
        public Document LoadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Document LoadFile(Stream file)
        {
            throw new NotImplementedException();
        }

        public IDocumentElement Parse(string context)
        {
            throw new NotImplementedException();
        }
    }
}