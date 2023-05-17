using System.Drawing;

namespace ImageConverter.Orchestrator;

public class FileConverter
{
    private readonly PluginFactory _factory = new();
    private string _defaultFilePath;

    public void Convert(string sourceFile, string objectiveExtension, string? outputPath)
    {
        _defaultFilePath = Path.GetDirectoryName(sourceFile) +
                           Path.DirectorySeparatorChar +
                           Path.GetFileNameWithoutExtension(sourceFile);
        var bytes = FileReader.ReadAllBytes(sourceFile);

        var imageReader = _factory.GetImageReaderInstance(bytes);
        var imageBytes = imageReader?.Read(bytes);
        if (imageBytes == null)
        {
            throw new Exception("Unable to read image file");
        }

        WriteToFile(imageBytes, objectiveExtension, outputPath);
    }

    private void WriteToFile(Color[,] image, string objectiveExtension, string? outputPath)
    {
        var imageWriter = _factory.GetImageWriterInstance(objectiveExtension);
        if (imageWriter == null)
        {
            throw new Exception($"No writer found for extension '{objectiveExtension}'");
        }

        var convertedImageBytes = imageWriter.Write(image);

        var outputFilePath = outputPath ?? $"{_defaultFilePath}.{imageWriter.FileExtention}";
        File.WriteAllBytes(outputFilePath, convertedImageBytes);
    }
}