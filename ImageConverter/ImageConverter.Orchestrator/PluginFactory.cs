using System.Reflection;
using ImageConverter.Protocols.Interfaces;

namespace ImageConverter.Orchestrator;

public class PluginFactory
{
    private readonly string _directory;
    private readonly IImageReader[] _imageReaders;
    private readonly IImageWriter[] _imageWriters;
    private readonly string _searchPattern;

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

    public IImageReader GetImageReaderInstance(byte[] fileContent)
    {
        throw new NotImplementedException();
    }

    public IImageWriter GetImageWriterInstance(string objectiveExtention)
    {
        throw new NotImplementedException();
    }
}