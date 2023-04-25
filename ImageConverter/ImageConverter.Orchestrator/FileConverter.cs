namespace ImageConverter.Orchestrator;

public static class FileConverter
{
    private static readonly PluginFactory PluginFactory = new();

    public static void Convert(byte[] imageBytes, string objectiveExtention)
    {
        var imageReader = PluginFactory.GetImageReaderInstance(imageBytes);
        var image = imageReader.Read(imageBytes);
        var imageWriter = PluginFactory.GetImageWriterInstance(objectiveExtention);
        imageWriter.Write(objectiveExtention, image);
    }
}