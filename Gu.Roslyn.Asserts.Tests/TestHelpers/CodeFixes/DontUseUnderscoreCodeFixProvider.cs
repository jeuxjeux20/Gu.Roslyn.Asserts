namespace Gu.Roslyn.Asserts.Tests.CodeFixes
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DontUseUnderscoreCodeFixProvider))]
    internal class DontUseUnderscoreCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(
                FieldNameMustNotBeginWithUnderscore.DiagnosticId,
                FieldNameMustNotBeginWithUnderscoreDifferentDiagnosticsForPublic.Id1,
                FieldNameMustNotBeginWithUnderscoreDifferentDiagnosticsForPublic.Id2);

        public override FixAllProvider GetFixAllProvider()
        {
            return CustomFixAllProviders.BatchFixer;
        }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var document = context.Document;
            var root = await document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            foreach (var diagnostic in context.Diagnostics)
            {
                var token = root.FindToken(diagnostic.Location.SourceSpan.Start);
                if (!string.IsNullOrEmpty(token.ValueText))
                {
                    var newName = token.ValueText.TrimStart(new[] { '_' });

                    if (string.IsNullOrEmpty(newName))
                    {
                        // The variable consisted of only underscores. In this case we cannot
                        // generate a valid variable name and thus will not offer a code fix.
                        continue;
                    }

                    context.RegisterCodeFix(
                        CodeAction.Create(
                            $"Rename to: {newName}",
                            cancellationToken => RenameHelper.RenameSymbolAsync(document, root, token, newName, cancellationToken),
                            nameof(DontUseUnderscoreCodeFixProvider)),
                        diagnostic);
                }
            }
        }
    }
}
