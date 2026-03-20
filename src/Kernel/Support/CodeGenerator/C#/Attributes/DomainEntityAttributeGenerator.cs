using Microsoft.CodeAnalysis;

//TODO: Put this note somewhere
//NOTE: to debug, set your startup as the CodeGenerator project, and hit f5
//TODO: Document all this.

namespace CodeGenerator
{
    [Generator]
    public class DomainEntityAttributeGenerator : IIncrementalGenerator
    {
        public const string DomainEntityAttributeName = "DomainEntity";
        public const string DomainEntityAttribute = $$"""
            namespace CodeGenerator
            {
                /// <summary>
                /// This attribute decorates an entity to be used for code generation.
                /// </summary>
                [AttributeUsage(AttributeTargets.Class)]
                public class {{DomainEntityAttributeName}} : Attribute
                {
                }
            }
            """;


        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(static ctx =>
            {
                ctx.AddSource($"{nameof(DomainEntityAttribute)}.g.cs", DomainEntityAttribute);
            });
        }
    }
}


