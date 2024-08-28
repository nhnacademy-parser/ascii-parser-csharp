using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentParser.DocumentSyntaxes;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;

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
                List<AsciiDocSyntax> list = _syntaxMap.GetValueOrDefault(index);

                foreach (AsciiDocSyntax syntax in list)
                {
                    if (syntax.Pattern.IsMatch(context))
                    {
                        return syntax;
                    }
                }
            }
            else
            {
                foreach (AsciiDocSyntax syntax in _inlineSyntax)
                {
                    Match match = syntax.Pattern.Match(context);

                    if (match.Success)
                    {
                        return syntax;
                    }
                }
            }

            return new AsciiDocSyntax(string.Empty, "", typeof(ParagraphElement));
        }
    }
}