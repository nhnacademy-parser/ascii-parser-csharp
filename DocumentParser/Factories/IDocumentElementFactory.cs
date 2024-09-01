using System;
using System.Text.RegularExpressions;
using DocumentParser.Domain;
using DocumentParser.Elements;

namespace DocumentParser.Factories
{
    public interface IDocumentElementFactory
    {
        IDocumentElement Create(Type type, GroupCollection groups);
    }
}