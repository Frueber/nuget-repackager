using NuGetRepackager.CommandLineArguments.Enums;
using NuGetRepackager.CommandLineArguments.Extensions;

namespace NuGetRepackager.CommandLineArguments;

/// <summary>
/// Used to interact with a given command line argument per expectations.
/// </summary>
internal sealed class CommandLineArgument
{
    /// <summary>
    /// The key, or "flag", for the command line argument.
    /// </summary>
    public CommandLineArgumentKey Key { get; }

    /// <summary>
    /// The optional value for the command line argument.  
    /// </summary>
    /// <remarks>
    /// It's possible that the command line argument was simply a key/flag with no value.
    /// </remarks>
    public string? Value { get; }

    /// <summary>
    /// Constructs the <see cref="CommandLineArgument"/> given a command line argument <see cref="string"/>.
    /// </summary>
    /// <param name="argument">The <see cref="CommandLineArgument"/>.</param>
    /// <exception cref="ArgumentException"></exception>
    public CommandLineArgument(string argument)
    {
        var parts = argument.Split(
            '=',
            2,
            StringSplitOptions.RemoveEmptyEntries
            | StringSplitOptions.TrimEntries
        );

        switch (parts.Length)
        {
            case 1:
                Key = parts[0].ToCommandLineArgumentKey();

                break;
            case 2:
                Key = parts[0].ToCommandLineArgumentKey();
                Value = parts[1];

                break;
            default:
                throw new ArgumentException($"Invalid argument syntax: {argument}.");
        }
    }
}
