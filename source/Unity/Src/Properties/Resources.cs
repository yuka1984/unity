
namespace Microsoft.Practices.Unity.Properties
{

    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal partial class Resources
    {

        private static CultureInfo culture = new CultureInfo("en-US");


        /// <summary>
        ///   Returns the cached Manager instance used by this class.
        /// </summary>
        internal static class Manager
        {

            public static IDictionary<CultureInfo, IDictionary<string, string>> resources = new Dictionary<CultureInfo, IDictionary<string, string>>
            {
                {
                    new CultureInfo("en-US"),
                    new Dictionary<string, string>
                    {
                        { "AmbiguousInjectionConstructor" , @"The type {0} has multiple constructors of length {1}. Unable to disambiguate." },
                        { "ArgumentMustNotBeEmpty" , @"The provided string argument must not be empty." },
                        { "BuildFailedException" , @"The current build operation (build key {2}) failed: {3} (Strategy type {0}, index {1})" },
                        { "CannotConstructAbstractClass" , @"The current type, {0}, is an abstract class and cannot be constructed. Are you missing a type mapping?" },
                        { "CannotConstructDelegate" , @"The current type, {0}, is delegate and cannot be constructed. Unity only supports resolving Func&lt;T&gt; and Func&lt;IEnumerable&lt;T&gt;&gt; by default." },
                        { "CannotConstructInterface" , @"The current type, {0}, is an interface and cannot be constructed. Are you missing a type mapping?" },
                        { "CannotExtractTypeFromBuildKey" , @"Cannot extract type from build key {0}." },
                        { "CannotInjectGenericMethod" , @"The method {0}.{1}({2}) is an open generic method. Open generic methods cannot be injected." },
                        { "CannotInjectIndexer" , @"The property {0} on type {1} is an indexer. Indexed properties cannot be injected." },
                        { "CannotInjectMethodWithOutParam" , @"The method {1} on type {0} has an out parameter. Injection cannot be performed." },
                        { "CannotInjectMethodWithOutParams" , @"The method {0}.{1}({2}) has at least one out parameter. Methods with out parameters cannot be injected." },
                        { "CannotInjectMethodWithRefParams" , @"The method {0}.{1}({2}) has at least one ref parameter.Methods with ref parameters cannot be injected." },
                        { "CannotInjectOpenGenericMethod" , @"The method {1} on type {0} is marked for injection, but it is an open generic method. Injection cannot be performed." },
                        { "CannotInjectStaticMethod" , @"The method {0}.{1}({2}) is static. Static methods cannot be injected." },
                        { "CannotResolveOpenGenericType" , @"The type {0} is an open generic type. An open generic type cannot be resolved." },
                        { "ConstructorArgumentResolveOperation" , "Resolving parameter \"{0}\" of constructor {1}" },
                        { "ConstructorParameterResolutionFailed" , @"The parameter {0} could not be resolved when attempting to call constructor {1}." },
                        { "ExceptionNullParameterValue" , @"Parameter type inference does not work for null values. Indicate the parameter type explicitly using a properly configured instance of the InjectionParameter or InjectionParameter&lt;T&gt; classes." },
                        { "InvokingConstructorOperation" , @"Calling constructor {0}" },
                        { "InvokingMethodOperation" , @"Calling method {0}.{1}" },
                        { "KeyAlreadyPresent" , @"An item with the given key is already present in the dictionary." },
                        { "LifetimeManagerInUse" , @"The lifetime manager is already registered. Lifetime managers cannot be reused, please create a new one." },
                        { "MarkerBuildPlanInvoked" , @"The override marker build plan policy has been invoked. This should never happen, looks like a bug in the container." },
                        { "MethodArgumentResolveOperation" , "Resolving parameter \"{0}\" of method {1}.{2}" },
                        { "MethodParameterResolutionFailed" , "The value for parameter \"{1}\" of method {0} could not be resolved. " },
                        { "MissingDependency" , @"Could not resolve dependency for build key {0}." },
                        { "MultipleInjectionConstructors" , @"The type {0} has multiple constructors marked with the InjectionConstructor attribute. Unable to disambiguate." },
                        { "MustHaveOpenGenericType" , @"The supplied type {0} must be an open generic type." },
                        { "MustHaveSameNumberOfGenericArguments" , @"The supplied type {0} does not have the same number of generic arguments as the target type {1}." },
                        { "NoConstructorFound" , @"The type {0} does not have an accessible constructor." },
                        { "NoMatchingGenericArgument" , "The type {0} does not have a generic argument named \"{1}\"" },
                        { "NoOperationExceptionReason" , @"while resolving" },
                        { "NoSuchConstructor" , @"The type {0} does not have a constructor that takes the parameters ({1})." },
                        { "NoSuchMethod" , @"The type {0} does not have a public method named {1} that takes the parameters ({2})." },
                        { "NoSuchProperty" , @"The type {0} does not contain an instance property named {1}." },
                        { "NotAGenericType" , "The type {0} is not a generic type, and you are attempting to inject a generic parameter named \"{1}\"." },
                        { "NotAnArrayTypeWithRankOne" , @"The type {0} is not an array type with rank 1, and you are attempting to use a [DependencyArray] attribute on a parameter or property with this type." },
                        { "OptionalDependenciesMustBeReferenceTypes" , @"Optional dependencies must be reference types. The type {0} is a value type." },
                        { "PropertyNotSettable" , @"The property {0} on type {1} is not settable." },
                        { "PropertyTypeMismatch" , @"The property {0} on type {1} is of type {2}, and cannot be injected with a value of type {3}." },
                        { "PropertyValueResolutionFailed" , "The value for the property \"{0}\" could not be resolved." },
                        { "ProvidedStringArgMustNotBeEmpty" , @"The provided string argument must not be empty." },
                        { "ResolutionFailed" , @"Resolution of the dependency failed, type = ""{0}"", name = ""{1}"".
                    Exception occurred while: {2}.
                    Exception is: {3} - {4}
                    -----------------------------------------------
                    At the time of the exception, the container was:
                    " },
                        { "ResolutionTraceDetail" , @"Resolving {0},{1}" },
                        { "ResolutionWithMappingTraceDetail" , @"Resolving {0},{1} (mapped from {2}, {3})" },
                        { "ResolvingPropertyValueOperation" , @"Resolving value for property {0}.{1}" },
                        { "SelectedConstructorHasRefParameters" , @"The constructor {1} selected for type {0} has ref or out parameters. Such parameters are not supported for constructor injection." },
                        { "SettingPropertyOperation" , @"Setting value for property {0}.{1}" },
                        { "TypeIsNotConstructable" , @"The type {0} cannot be constructed. You must configure the container to supply this value." },
                        { "TypesAreNotAssignable" , @"The type {1} cannot be assigned to variables of type {0}." },
                        { "UnknownType" , @"&lt;unknown&gt;" },
                        { "DuplicateTypeMappingException", "An attempt to override an existing mapping was detected for type {1} with name \"{0}\", currently mapped to type {2}, to type {3}." },
                        { "ExceptionNullAssembly", "The set of assemblies contains a null element." }
                    }
                }
            };

            public static string GetString(string key, CultureInfo culture)
            {
                var cultureKey = culture;
                if (!resources.ContainsKey(culture))
                    cultureKey = resources.Keys.First();
                return resources[cultureKey][key];
            }
        }


        /// <summary>
        ///   Looks up a localized string similar to The type {0} has multiple constructors of length {1}. Unable to disambiguate..
        /// </summary>
        internal static string AmbiguousInjectionConstructor
        {
            get
            {
                return Manager.GetString("AmbiguousInjectionConstructor", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The provided string argument must not be empty..
        /// </summary>
        internal static string ArgumentMustNotBeEmpty
        {
            get
            {
                return Manager.GetString("ArgumentMustNotBeEmpty", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current build operation (build key {2}) failed: {3} (Strategy type {0}, index {1}).
        /// </summary>
        internal static string BuildFailedException
        {
            get
            {
                return Manager.GetString("BuildFailedException", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current type, {0}, is an abstract class and cannot be constructed. Are you missing a type mapping?.
        /// </summary>
        internal static string CannotConstructAbstractClass
        {
            get
            {
                return Manager.GetString("CannotConstructAbstractClass", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current type, {0}, is delegate and cannot be constructed. Unity only supports resolving Func&lt;T&gt; and Func&lt;IEnumerable&lt;T&gt;&gt; by default..
        /// </summary>
        internal static string CannotConstructDelegate
        {
            get
            {
                return Manager.GetString("CannotConstructDelegate", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current type, {0}, is an interface and cannot be constructed. Are you missing a type mapping?.
        /// </summary>
        internal static string CannotConstructInterface
        {
            get
            {
                return Manager.GetString("CannotConstructInterface", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot extract type from build key {0}..
        /// </summary>
        internal static string CannotExtractTypeFromBuildKey
        {
            get
            {
                return Manager.GetString("CannotExtractTypeFromBuildKey", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The method {0}.{1}({2}) is an open generic method. Open generic methods cannot be injected..
        /// </summary>
        internal static string CannotInjectGenericMethod
        {
            get
            {
                return Manager.GetString("CannotInjectGenericMethod", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The property {0} on type {1} is an indexer. Indexed properties cannot be injected..
        /// </summary>
        internal static string CannotInjectIndexer
        {
            get
            {
                return Manager.GetString("CannotInjectIndexer", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The method {1} on type {0} has an out parameter. Injection cannot be performed..
        /// </summary>
        internal static string CannotInjectMethodWithOutParam
        {
            get
            {
                return Manager.GetString("CannotInjectMethodWithOutParam", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The method {0}.{1}({2}) has at least one out parameter. Methods with out parameters cannot be injected..
        /// </summary>
        internal static string CannotInjectMethodWithOutParams
        {
            get
            {
                return Manager.GetString("CannotInjectMethodWithOutParams", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The method {0}.{1}({2}) has at least one ref parameter.Methods with ref parameters cannot be injected..
        /// </summary>
        internal static string CannotInjectMethodWithRefParams
        {
            get
            {
                return Manager.GetString("CannotInjectMethodWithRefParams", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The method {1} on type {0} is marked for injection, but it is an open generic method. Injection cannot be performed..
        /// </summary>
        internal static string CannotInjectOpenGenericMethod
        {
            get
            {
                return Manager.GetString("CannotInjectOpenGenericMethod", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The method {0}.{1}({2}) is static. Static methods cannot be injected..
        /// </summary>
        internal static string CannotInjectStaticMethod
        {
            get
            {
                return Manager.GetString("CannotInjectStaticMethod", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} is an open generic type. An open generic type cannot be resolved..
        /// </summary>
        internal static string CannotResolveOpenGenericType
        {
            get
            {
                return Manager.GetString("CannotResolveOpenGenericType", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Resolving parameter &quot;{0}&quot; of constructor {1}.
        /// </summary>
        internal static string ConstructorArgumentResolveOperation
        {
            get
            {
                return Manager.GetString("ConstructorArgumentResolveOperation", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The parameter {0} could not be resolved when attempting to call constructor {1}..
        /// </summary>
        internal static string ConstructorParameterResolutionFailed
        {
            get
            {
                return Manager.GetString("ConstructorParameterResolutionFailed", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Parameter type inference does not work for null values. Indicate the parameter type explicitly using a properly configured instance of the InjectionParameter or InjectionParameter&lt;T&gt; classes..
        /// </summary>
        internal static string ExceptionNullParameterValue
        {
            get
            {
                return Manager.GetString("ExceptionNullParameterValue", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Calling constructor {0}.
        /// </summary>
        internal static string InvokingConstructorOperation
        {
            get
            {
                return Manager.GetString("InvokingConstructorOperation", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Calling method {0}.{1}.
        /// </summary>
        internal static string InvokingMethodOperation
        {
            get
            {
                return Manager.GetString("InvokingMethodOperation", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An item with the given key is already present in the dictionary..
        /// </summary>
        internal static string KeyAlreadyPresent
        {
            get
            {
                return Manager.GetString("KeyAlreadyPresent", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The lifetime manager is already registered. Lifetime managers cannot be reused, please create a new one..
        /// </summary>
        internal static string LifetimeManagerInUse
        {
            get
            {
                return Manager.GetString("LifetimeManagerInUse", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The override marker build plan policy has been invoked. This should never happen, looks like a bug in the container..
        /// </summary>
        internal static string MarkerBuildPlanInvoked
        {
            get
            {
                return Manager.GetString("MarkerBuildPlanInvoked", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Resolving parameter &quot;{0}&quot; of method {1}.{2}.
        /// </summary>
        internal static string MethodArgumentResolveOperation
        {
            get
            {
                return Manager.GetString("MethodArgumentResolveOperation", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The value for parameter &quot;{1}&quot; of method {0} could not be resolved. .
        /// </summary>
        internal static string MethodParameterResolutionFailed
        {
            get
            {
                return Manager.GetString("MethodParameterResolutionFailed", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Could not resolve dependency for build key {0}..
        /// </summary>
        internal static string MissingDependency
        {
            get
            {
                return Manager.GetString("MissingDependency", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} has multiple constructors marked with the InjectionConstructor attribute. Unable to disambiguate..
        /// </summary>
        internal static string MultipleInjectionConstructors
        {
            get
            {
                return Manager.GetString("MultipleInjectionConstructors", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The supplied type {0} must be an open generic type..
        /// </summary>
        internal static string MustHaveOpenGenericType
        {
            get
            {
                return Manager.GetString("MustHaveOpenGenericType", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The supplied type {0} does not have the same number of generic arguments as the target type {1}..
        /// </summary>
        internal static string MustHaveSameNumberOfGenericArguments
        {
            get
            {
                return Manager.GetString("MustHaveSameNumberOfGenericArguments", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} does not have an accessible constructor..
        /// </summary>
        internal static string NoConstructorFound
        {
            get
            {
                return Manager.GetString("NoConstructorFound", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} does not have a generic argument named &quot;{1}&quot;.
        /// </summary>
        internal static string NoMatchingGenericArgument
        {
            get
            {
                return Manager.GetString("NoMatchingGenericArgument", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to while resolving.
        /// </summary>
        internal static string NoOperationExceptionReason
        {
            get
            {
                return Manager.GetString("NoOperationExceptionReason", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} does not have a constructor that takes the parameters ({1})..
        /// </summary>
        internal static string NoSuchConstructor
        {
            get
            {
                return Manager.GetString("NoSuchConstructor", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} does not have a public method named {1} that takes the parameters ({2})..
        /// </summary>
        internal static string NoSuchMethod
        {
            get
            {
                return Manager.GetString("NoSuchMethod", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} does not contain an instance property named {1}..
        /// </summary>
        internal static string NoSuchProperty
        {
            get
            {
                return Manager.GetString("NoSuchProperty", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} is not a generic type, and you are attempting to inject a generic parameter named &quot;{1}&quot;..
        /// </summary>
        internal static string NotAGenericType
        {
            get
            {
                return Manager.GetString("NotAGenericType", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} is not an array type with rank 1, and you are attempting to use a [DependencyArray] attribute on a parameter or property with this type..
        /// </summary>
        internal static string NotAnArrayTypeWithRankOne
        {
            get
            {
                return Manager.GetString("NotAnArrayTypeWithRankOne", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Optional dependencies must be reference types. The type {0} is a value type..
        /// </summary>
        internal static string OptionalDependenciesMustBeReferenceTypes
        {
            get
            {
                return Manager.GetString("OptionalDependenciesMustBeReferenceTypes", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The property {0} on type {1} is not settable..
        /// </summary>
        internal static string PropertyNotSettable
        {
            get
            {
                return Manager.GetString("PropertyNotSettable", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The property {0} on type {1} is of type {2}, and cannot be injected with a value of type {3}..
        /// </summary>
        internal static string PropertyTypeMismatch
        {
            get
            {
                return Manager.GetString("PropertyTypeMismatch", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The value for the property &quot;{0}&quot; could not be resolved..
        /// </summary>
        internal static string PropertyValueResolutionFailed
        {
            get
            {
                return Manager.GetString("PropertyValueResolutionFailed", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The provided string argument must not be empty..
        /// </summary>
        internal static string ProvidedStringArgMustNotBeEmpty
        {
            get
            {
                return Manager.GetString("ProvidedStringArgMustNotBeEmpty", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Resolution of the dependency failed, type = &quot;{0}&quot;, name = &quot;{1}&quot;.
        ///Exception occurred while: {2}.
        ///Exception is: {3} - {4}
        ///-----------------------------------------------
        ///At the time of the exception, the container was:
        ///.
        /// </summary>
        internal static string ResolutionFailed
        {
            get
            {
                return Manager.GetString("ResolutionFailed", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Resolving {0},{1}.
        /// </summary>
        internal static string ResolutionTraceDetail
        {
            get
            {
                return Manager.GetString("ResolutionTraceDetail", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Resolving {0},{1} (mapped from {2}, {3}).
        /// </summary>
        internal static string ResolutionWithMappingTraceDetail
        {
            get
            {
                return Manager.GetString("ResolutionWithMappingTraceDetail", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Resolving value for property {0}.{1}.
        /// </summary>
        internal static string ResolvingPropertyValueOperation
        {
            get
            {
                return Manager.GetString("ResolvingPropertyValueOperation", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The constructor {1} selected for type {0} has ref or out parameters. Such parameters are not supported for constructor injection..
        /// </summary>
        internal static string SelectedConstructorHasRefParameters
        {
            get
            {
                return Manager.GetString("SelectedConstructorHasRefParameters", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Setting value for property {0}.{1}.
        /// </summary>
        internal static string SettingPropertyOperation
        {
            get
            {
                return Manager.GetString("SettingPropertyOperation", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {0} cannot be constructed. You must configure the container to supply this value..
        /// </summary>
        internal static string TypeIsNotConstructable
        {
            get
            {
                return Manager.GetString("TypeIsNotConstructable", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type {1} cannot be assigned to variables of type {0}..
        /// </summary>
        internal static string TypesAreNotAssignable
        {
            get
            {
                return Manager.GetString("TypesAreNotAssignable", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to &lt;unknown&gt;.
        /// </summary>
        internal static string UnknownType
        {
            get
            {
                return Manager.GetString("UnknownType", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An attempt to override an existing mapping was detected for type {1} with name &quot;{0}&quot;, currently mapped to type {2}, to type {3}..
        /// </summary>
        internal static string DuplicateTypeMappingException
        {
            get
            {
                return Manager.GetString("DuplicateTypeMappingException", culture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The set of assemblies contains a null element..
        /// </summary>
        internal static string ExceptionNullAssembly
        {
            get
            {
                return Manager.GetString("ExceptionNullAssembly", culture);
            }
        }
    }
}
