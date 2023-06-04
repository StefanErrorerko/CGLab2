namespace ImageWriter;

public class FileWriter
{
    private readonly string _defaultFilePath;
    private readonly IImageWriter _imageWriter;

    public FileWriter(IImageWriter imageWriter)
    {
        _imageWriter = imageWriter;
        _defaultFilePath = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}rendered";
    }

    public void WriteToFile(float[,] traceResult, string? outputPath)
    {
        var pixels = TraceResultConverter.ConvertToGrayscalePixels(traceResult);
        var convertedImageBytes = _imageWriter.Write(pixels);

        var outputFilePath = outputPath ?? $"{_defaultFilePath}.{_imageWriter.FileExtention}";
        File.WriteAllBytes(outputFilePath, convertedImageBytes);
    }
}