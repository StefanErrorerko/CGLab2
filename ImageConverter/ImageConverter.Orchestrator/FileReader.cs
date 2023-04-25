namespace ImageConverter.Orchestrator;

public class FileReader
{
    public static byte[] ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("File is not found");
        }

        return File.ReadAllBytes(path);
    }
}