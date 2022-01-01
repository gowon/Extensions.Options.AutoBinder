using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Extensions.Options.AutoBinder")]
[assembly: AssemblyProduct("Extensions.Options.AutoBinder")]
[assembly: AssemblyCompany("Gowon, Ltd.")]
[assembly: AssemblyCopyright("Copyright © Gowon, Ltd. 2022")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

#pragma warning disable CS0436 // Type conflicts with imported type
[assembly: AssemblyVersion(ThisAssembly.Version)]
[assembly: AssemblyFileVersion(ThisAssembly.SimpleVersion)]
[assembly: AssemblyInformationalVersion(ThisAssembly.InformationalVersion)]
#pragma warning restore CS0436 // Type conflicts with imported type

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1b87d88d-8b88-49ef-ab49-7a498569605c")]

// ReSharper disable once CheckNamespace
internal partial class ThisAssembly
{
    /// <summary>
    ///     Simple release-like version number, like 4.0.1 for a cycle 5, SR1 build.
    /// </summary>
    public const string SimpleVersion =
        Git.SemVer.Major + "." + Git.SemVer.Minor + "." + Git.SemVer.Patch;

    /// <summary>
    ///     Full version, including commits since base version file, like 4.0.1.598
    /// </summary>
    public const string Version = SimpleVersion;

    /// <summary>
    ///     Full version, plus branch and commit short sha, like 4.0.1.598-cycle6+39cf84e
    /// </summary>
#if ALPHA_BUILD
    public const string InformationalVersion = Version + "-alpha" + "+" + Git.Commit;
#else
    public const string InformationalVersion = Version + "+" + Git.Commit;

#endif
}