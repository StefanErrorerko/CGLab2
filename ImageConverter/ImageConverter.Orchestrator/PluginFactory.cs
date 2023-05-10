using System.Reflection;
using ImageConverter.Protocols.Interfaces;

namespace ImageConverter.Orchestrator;

public class PluginFactory
{
    private const string Directory = "..\\..\\..\\..\\..\\..\\..\\..\\..\\..\\net6.0";
    private const string SearchPattern = "*.dll";

    private readonly List<IImageReader> _imageReaders;
    private readonly List<IImageWriter> _imageWriters;

    public PluginFactory()
    {
        var assemblies = LoadAssemblies();

        _imageReaders = InstantiatePlugins<IImageReader>(assemblies);
        _imageWriters = InstantiatePlugins<IImageWriter>(assemblies);
    }

    private static List<Assembly> LoadAssemblies()
    {
        var assemblies = System.IO.Directory.GetFiles(Directory, SearchPattern)
            .Select(Assembly.LoadFrom)
            .ToList();
        return assemblies;
    }

    private static List<T> InstantiatePlugins<T>(IEnumerable<Assembly> assemblies) where T : class
    {
        return assemblies.SelectMany(s => s.GetExportedTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass)
            .Select(type => (T)Activator.CreateInstance(type)!)
            .ToList();
    }

    public IImageReader GetImageReaderInstance(byte[] bytes)
    {
        return _imageReaders.FirstOrDefault(reader => reader.ValidateHeader(bytes));
    }

    public IImageWriter GetImageWriterInstance(string objectiveExtention)
    {
        return _imageWriters.FirstOrDefault(writer => writer.FileExtention == objectiveExtention);
    }

    public string[] GetSupportedReadersExtensions()
    {
        return _imageReaders.Select(reader => $".{reader.FileExtention}").ToArray();
    }

    public string[] GetSupportedWritersExtensions()
    {
        return _imageWriters.Select(writer => $".{writer.FileExtention}").ToArray();
    }
}