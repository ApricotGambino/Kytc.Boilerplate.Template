using Microsoft.CodeAnalysis;

namespace KytcBPAnalyzer
{
    public static class DiagnosticCategories
    {
        public const string Architecture = "Architecture";
    }
    public static class KytcBPDiagnosticDescriptors
    {
#pragma warning disable RS2008 // Enable analyzer release tracking
        public static readonly DiagnosticDescriptor KYTCBP001 = new(nameof(KYTCBP001), "Entity should inherit from BaseEntity", "Entity {0} is using attribute [DomainEntity] but doesn't inherit from from BaseEntity", DiagnosticCategories.Architecture, DiagnosticSeverity.Error, true, "It's important that if you've decorated an entity with [DomainEntity] that you also inherit from BaseEntity, as the architecture expects that base entity as a type for many things, including code generation.");
        public static readonly DiagnosticDescriptor KYTCBP002 = new(nameof(KYTCBP002), "Entity should implement interface fields", "Implement an interface named 'I{0}Fields' with all the members of this class", DiagnosticCategories.Architecture, DiagnosticSeverity.Error, true, "This entity inherits from BaseEntity, but doesn't implement an interface named 'IENTITYNAMEFields' with all the members of this entity. This allows validation and other generators to accurately build and map required objects to support this entity.");
        public static readonly DiagnosticDescriptor KYTCBP003 = new(nameof(KYTCBP003), "EntityField interface should use the [DomainEntityFields] attribute", "Interface {0} needs to have the attribute [DomainEntityFields]", DiagnosticCategories.Architecture, DiagnosticSeverity.Error, true, "This interface seems to be an IENTITYNAMEField meant to describe the members of a Domain Entity. The [DomainEntityFields] attribute is used in the pipline of the application to find validators for this entity.");
#pragma warning restore RS2008 // Enable analyzer release tracking
    }
}
