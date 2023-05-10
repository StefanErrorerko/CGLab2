namespace ImageConverter.Orchestrator;

public class FileReader
{
    public static byte[] ReadAllBytes(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Source file is not found");
        }

        return File.ReadAllBytes(path);
    }
}