namespace Extensions.Options.AutoBinder
{
    using System;

    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AutoBindAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public string[] Names { get; }

        /// <summary>
        /// </summary>
        /// <param name="names"></param>
        public AutoBindAttribute(params string[] names)
        {
            Names = names;
        }
    }
}