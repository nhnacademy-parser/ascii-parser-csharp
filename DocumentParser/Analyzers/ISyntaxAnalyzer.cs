using DocumentParser.DocumentSyntaxes;
using DocumentParser.Elements.Implementations;

namespace DocumentParser.Analyzers
{
	public interface IDocumentSyntaxAnalyzer
	{
		IDocumentSyntax Analyze(string context);

	}
}

