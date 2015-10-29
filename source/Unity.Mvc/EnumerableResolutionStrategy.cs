namespace WebApplication6.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.ObjectBuilder2;
    using System.Reflection;

    /// <summary>
    /// Allows to resolve generic IEnumerable`1 types
    /// </summary>
    internal class EnumerableResolutionStrategy : IBuilderStrategy
    {
        /// <summary>
        /// This method replaces the build key used to resolve <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public void PreBuildUp(IBuilderContext context)
        {
            if (context.BuildKey.Type.GetGenericArguments().Length > 0 && context.BuildKey.Type.FullName.StartsWith("System.Collections.Generic.IEnumerable"))
            {
                var arrayType = context.BuildKey.Type.GetGenericArguments()[0].MakeArrayType();
                context.BuildKey = new NamedTypeBuildKey(arrayType, context.BuildKey.Name);
            }
        }

        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public void PostBuildUp(IBuilderContext context)
        {
        }

        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public void PreTearDown(IBuilderContext context)
        {
        }

        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public void PostTearDown(IBuilderContext context)
        {
        }
    }
}