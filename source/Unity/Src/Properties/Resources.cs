using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Practices.Unity.Properties
{
    public static partial class Resources
    {
        public static string AmbiguousInjectionConstructor { get { return "The type {0} has multiple constructors of length {1}. Unable to disambiguate."; }}
        public static string ArgumentMustNotBeEmpty { get { return "The provided string argument must not be empty."; }}
        public static string BuildFailedException { get { return "The current build operation (build key {2}) failed: {3} (Strategy type {0}, index {1})"; }}
        public static string CannotConstructAbstractClass { get { return "The current type, {0}, is an abstract class and cannot be constructed. Are you missing a type mapping?"; }}
        public static string CannotConstructDelegate { get { return "The current type, {0}, is delegate and cannot be constructed. Unity only supports resolving Func&lt;T&gt; and Func&lt;IEnumerable&lt;T&gt;&gt; by default."; }}
        public static string CannotConstructInterface { get { return "The current type, {0}, is an interface and cannot be constructed. Are you missing a type mapping?"; }}
        public static string CannotExtractTypeFromBuildKey { get { return "Cannot extract type from build key {0}."; }}
        public static string CannotInjectGenericMethod { get { return "The method {0}.{1}({2}) is an open generic method. Open generic methods cannot be injected."; }}
        public static string CannotInjectIndexer { get { return "The property {0} on type {1} is an indexer. Indexed properties cannot be injected."; }}
        public static string CannotInjectMethodWithOutParam { get { return "The method {1} on type {0} has an out parameter. Injection cannot be performed."; }}
        public static string CannotInjectMethodWithOutParams { get { return "The method {0}.{1}({2}) has at least one out parameter. Methods with out parameters cannot be injected."; }}
        public static string CannotInjectMethodWithRefParams { get { return "The method {0}.{1}({2}) has at least one ref parameter.Methods with ref parameters cannot be injected."; }}
        public static string CannotInjectOpenGenericMethod { get { return "The method {1} on type {0} is marked for injection, but it is an open generic method. Injection cannot be performed."; }}
        public static string CannotInjectStaticMethod { get { return "The method {0}.{1}({2}) is static. Static methods cannot be injected."; }}
        public static string CannotResolveOpenGenericType { get { return "The type {0} is an open generic type. An open generic type cannot be resolved."; }}
        public static string ConstructorArgumentResolveOperation { get { return "Resolving parameter '{0}' of constructor {1}"; }}
        public static string ConstructorParameterResolutionFailed { get { return "The parameter {0} could not be resolved when attempting to call constructor {1}."; }}
        public static string ExceptionNullParameterValue { get { return "Parameter type inference does not work for null values. Indicate the parameter type explicitly using a properly configured instance of the InjectionParameter or InjectionParameter&lt;T&gt; classes."; }}
        public static string InvokingConstructorOperation  { get { return "Calling constructor {0}"; }}
        public static string InvokingMethodOperation { get { return "Calling method {0}.{1}"; }}
        public static string KeyAlreadyPresent { get { return "An item with the given key is already present in the dictionary."; }}
        public static string LifetimeManagerInUse { get { return "The lifetime manager is already registered. Lifetime managers cannot be reused, please create a new one."; }}
        public static string MarkerBuildPlanInvoked { get { return "The override marker build plan policy has been invoked. This should never happen, looks like a bug in the container."; }}
        public static string MethodArgumentResolveOperation { get { return "Resolving parameter '{0}' of method {1}.{2}"; }}
        public static string MethodParameterResolutionFailed { get { return "The value for parameter '{1}' of method {0} could not be resolved."; }}
        public static string MissingDependency { get { return "Could not resolve dependency for build key {0}."; }}
        public static string MultipleInjectionConstructors { get { return "The type {0} has multiple constructors marked with the InjectionConstructor attribute. Unable to disambiguate."; }}
        public static string MustHaveOpenGenericType { get { return "The supplied type {0} must be an open generic type."; }}
        public static string MustHaveSameNumberOfGenericArguments { get { return "The supplied type {0} does not have the same number of generic arguments as the target type {1}."; }}
        public static string NoConstructorFound { get { return "The type {0} does not have an accessible constructor."; }}
        public static string NoMatchingGenericArgument { get { return "The type {0} does not have a generic argument named '{1}'"; }}
        public static string NoOperationExceptionReason { get { return "while resolving"; }}
        public static string NoSuchConstructor { get { return "The type {0} does not have a constructor that takes the parameters ({1})."; }}
        public static string NoSuchMethod { get { return "The type {0} does not have a public method named {1} that takes the parameters ({2})."; }}
        public static string NoSuchProperty { get { return "The type {0} does not contain an instance property named {1}."; }}
        public static string NotAGenericType { get { return "The type {0} is not a generic type, and you are attempting to inject a generic parameter named '{1}'."; }}
        public static string NotAnArrayTypeWithRankOne { get { return "The type {0} is not an array type with rank 1, and you are attempting to use a [DependencyArray] attribute on a parameter or property with this type."; }}
        public static string OptionalDependenciesMustBeReferenceTypes { get { return "Optional dependencies must be reference types. The type {0} is a value type."; }}
        public static string PropertyNotSettable { get { return "The property {0} on type {1} is not settable."; }}
        public static string PropertyTypeMismatch { get { return "The property {0} on type {1} is of type {2}, and cannot be injected with a value of type {3}."; }}
        public static string PropertyValueResolutionFailed { get { return "The value for the property '{0}' could not be resolved."; }}
        public static string ProvidedStringArgMustNotBeEmpty { get { return "The provided string argument must not be empty."; }}
        public static string ResolutionFailed { get { return @"Resolution of the dependency failed, type = '{0}', name = '{1}'.
    Exception occurred while: {2}.
    Exception is: {3} - {4}
    -----------------------------------------------
    At the time of the exception, the container was:"; }}
        public static string ResolutionTraceDetail { get { return "Resolving {0},{1}"; }}
        public static string ResolutionWithMappingTraceDetail { get { return "Resolving {0},{1} (mapped from {2}, {3})"; }}
        public static string ResolvingPropertyValueOperation { get { return "Resolving value for property {0}.{1}"; }}
        public static string SelectedConstructorHasRefParameters { get { return "The constructor {1} selected for type {0} has ref or out parameters. Such parameters are not supported for constructor injection."; }}
        public static string SettingPropertyOperation { get { return "Setting value for property {0}.{1}"; }}
        public static string TypeIsNotConstructable { get { return "The type {0} cannot be constructed. You must configure the container to supply this value."; }}
        public static string TypesAreNotAssignable { get { return "The type {1} cannot be assigned to variables of type {0}."; }}
        public static string UnknownType { get { return "<unknown>"; }}

    }
}
