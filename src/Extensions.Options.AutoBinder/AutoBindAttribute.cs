namespace Extensions.Options.AutoBinder;

/// <summary>
///     Specifies the keys to match and bind the class to data in
///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" />.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AutoBindAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="T:AutoBindAttribute" /> class with the specified list of keys.
    /// </summary>
    /// <param name="keys">
    ///     The list of keys to match when binding the class to
    ///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" />.
    /// </param>
    public AutoBindAttribute(params string[] keys)
    {
        Keys = keys;
    }

    /// <summary>
    ///     The list of keys to match when binding the class to
    ///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" />.
    /// </summary>
    public string[] Keys { get; }
}