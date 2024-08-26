using DocumentParser.DocumentSyntaxes;

namespace DocumentParser.Analyzers
{
	public interface IDocumentSyntaxAnalyzer
	{
		IDocumentSyntax Analyze(string context);

	}
}

