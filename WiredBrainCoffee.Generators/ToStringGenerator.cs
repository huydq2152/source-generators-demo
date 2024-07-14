using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WiredBrainCoffee.Generators.Model;

namespace WiredBrainCoffee.Generators;

[Generator]
public class ToStringGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classes = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, _) => IsSyntaxTarget(node),
            transform: static (syntaxContext, _) => GetSemanticTarget(syntaxContext))
            .Where(static (target) => target is not null);

        context.RegisterSourceOutput(classes,
            static (productionContext, syntax) => Execute(productionContext, syntax));

        context.RegisterPostInitializationOutput(static (initializationContext) => PostInitializationOutput(initializationContext));
    }

    private static bool IsSyntaxTarget(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
    }
    private static ClassToGenerate? GetSemanticTarget(GeneratorSyntaxContext syntaxContext)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)syntaxContext.Node;
        var classSymbol = syntaxContext.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        var attributeSymbol = syntaxContext.SemanticModel.Compilation
            .GetTypeByMetadataName("WiredBrainCoffee.Generators.GenerateToStringAttribute");

        if (classSymbol is not null && attributeSymbol is not null)
        {
            foreach (var attributeData in classSymbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass, attributeSymbol))
                {
                    var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
                    var className = classSymbol.Name;
                    var propertyNames = new List<string>();

                    foreach (var member in classSymbol.GetMembers())
                    {
                        if (member.Kind == SymbolKind.Property && member.DeclaredAccessibility == Accessibility.Public)
                        {
                            propertyNames.Add(member.Name);
                        }
                    }

                    return new ClassToGenerate(namespaceName, className, propertyNames);
                }
            }
        }

        return null;
    }

    private static void PostInitializationOutput(IncrementalGeneratorPostInitializationContext initializationContext)
    {
        initializationContext.AddSource("WiredBrainCoffee.Generators.GenerateToStringAttribute.g.cs",
            @"namespace WiredBrainCoffee.Generators
{
    internal class GenerateToStringAttribute : System.Attribute
    {
           
    }
}");
    }

    private static void Execute(SourceProductionContext context, ClassToGenerate? classToGenerate)
    {
        if(classToGenerate is null)
        {
            return;
        }

        var namespaceName = classToGenerate.NamespaceName;
        var className = classToGenerate.ClassName;
        var fileName = $"{namespaceName}.{className}.g.cs";

        var stringBuilder = new StringBuilder();
        stringBuilder.Append($@"namespace {namespaceName}
{{
    partial class {className}
    {{
        public override string ToString()
        {{
            return $""");
        var first = true;
        foreach (var propertyName in classToGenerate.PropertyNames)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                stringBuilder.Append("; ");
            }

            stringBuilder.Append($"{propertyName}: {{{propertyName}}}");
        }

        stringBuilder.Append($@""";
        }}
    }}
}}
");

        context.AddSource(fileName, stringBuilder.ToString());
    }
}