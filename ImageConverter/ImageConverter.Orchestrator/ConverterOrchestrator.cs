namespace ImageConverter.Orchestrator;

public class ConverterOrchestrator
{
    public static void Convert(string sourcePath, string objectiveExtention)
    {
        var fileContent = FileReader.ReadFile(sourcePath);
        FileConverter.Convert(fileContent, objectiveExtention);
    }
}