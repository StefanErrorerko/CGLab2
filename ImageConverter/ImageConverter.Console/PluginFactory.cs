using System.IO;
using System.Reflection;
using ImageConverter.Protocols.Interfaces;

namespace ImageConverter.Console;

public class PluginFactory
{
    private readonly string _directory;
    private readonly string _searchPattern;
    private readonly IImageWriter[] _imageWriters;
    private readonly IImageReader[] _imageReaders;

    public PluginFactory()
    {
        _directory = Directory.GetCurrentDirectory();
        _searchPattern = "*.dll";

        var assemblies = Directory.GetFiles(_directory, _searchPattern)
            .Select(Assembly.LoadFrom)
            .ToList();

        _imageReaders = assemblies.SelectMany(s => s.GetExportedTypes())
            .Where(type => typeof(IImageReader).IsAssignableFrom(type) && type.IsClass)
            .Select(type => (IImageReader)Activator.CreateInstance(type)!)
            .ToArray();

        _imageWriters = assemblies.SelectMany(s => s.GetExportedTypes())
            .Where(type => typeof(IImageWriter).IsAssignableFrom(type) && type.IsClass)
            .Select(type => (IImageWriter)Activator.CreateInstance(type)!)
            .ToArray();
    }
}