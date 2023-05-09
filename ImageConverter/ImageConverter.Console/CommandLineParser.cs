namespace ImageConverter.Console;

internal class CommandLineParser
{
    private readonly Dictionary<string, string> _arguments;

    public CommandLineParser(string[] args)
    {
        _arguments = new Dictionary<string, string>();

        foreach (var arg in args)
        {
            var equalsIndex = arg.IndexOf('=');
            if (equalsIndex == -1) throw new ArgumentException($"Invalid argument: {arg}");

            var key = arg.Substring(0, equalsIndex);
            var value = arg.Substring(equalsIndex + 1);

            _arguments[key] = value;
        }
    }

    public string GetArgument(string key)
    {
        if (!_arguments.TryGetValue(key, out var value)) throw new ArgumentException($"Argument not found: {key}");

        return value;
    }

    public bool HasArgument(string key)
    {
        return _arguments.ContainsKey(key);
    }
}