using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentParser.DocumentSyntaxes;
using DocumentParser.DocumentSyntaxes.implementations;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Elements.Implementations.Inlines;

namespace DocumentParser.Analyzers.Implementations
{
    public class AsciiDocsAnalyzer : IDocumentSyntaxAnalyzer
    {
        Dictionary<string, List<AsciiDocSyntax>> _syntaxMap;

        List<AsciiDocSyntax> _inlineSyntax;

        public AsciiDocsAnalyzer()
        {
        }

        public void Init()
        {
            _syntaxMap = new Dictionary<string, List<AsciiDocSyntax>>();
            _inlineSyntax = new List<AsciiDocSyntax>();

            foreach (AsciiDocSyntax syntax in new AsciiDocSyntax())
            {
                if (string.IsNullOrEmpty(syntax.Index))
                {
                    _inlineSyntax.Add(syntax);
                    continue;
                }

                _syntaxMap.TryAdd(syntax.Index, _syntaxMap.GetValueOrDefault(syntax.Index, new List<AsciiDocSyntax>()));
                _syntaxMap.GetValueOrDefault(syntax.Index).Add(syntax);
            }
        }

        public IDocumentSyntax Analyze(string context)
        {
            string index = context.Length == 0 ? "" : context[..1];

            if (_syntaxMap.ContainsKey(index))
            {
                List<AsciiDocSyntax> list = _syntaxMap[index];
                AsciiDocSyntax s = list.Find(s => s.Pattern.IsMatch(context));
                if (s != null)
                {
                    return s;
                }
            }

            return _inlineSyntax.Find(s => s.Pattern.IsMatch(context));
        }
    }
}